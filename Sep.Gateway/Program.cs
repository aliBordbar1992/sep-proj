using Sep.Gateway.Adapters;
using Sep.Gateway.PaymentProviders;
using Sep.Gateway.Services;
using TransferDto = Sep.Gateway.Services.TransferDto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();


builder.Services.AddTransient<IMellatBankPaymentProvider, MellatBankPaymentProvider>();
builder.Services.AddTransient<ISamanBankPaymentProvider, SamanBankPaymentProvider>();
builder.Services.AddTransient<IAyandehBankPaymentProvider, AyandehBankPaymentProvider>();

builder.Services.AddTransient<IBankAdapter, MellatBankAdapter>();
builder.Services.AddTransient<IBankAdapter, SamanBankAdapter>();
builder.Services.AddTransient<IBankAdapter, AyandehBankAdapter>();

builder.Services.AddSingleton<IBankAdapterResolver, BankAdapterResolver>();

builder.Services.AddTransient<ITransferService, TransferService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


await using (var scope = app.Services.CreateAsyncScope())
{
    // we get all adapters at startup and cache each
    // bank identification number for future usage
    // for simplicity we use a dictionary here
    var allAdapters = scope.ServiceProvider.GetServices<IBankAdapter>();
    var adapterResolver = scope.ServiceProvider.GetService<IBankAdapterResolver>();
    foreach (var adapter in allAdapters)
    {
        var response = await adapter.GetIdentificationNumberAsync();

        if (response.IsSuccess)
            adapterResolver!.RegisterAdapter(response.Data, adapter);
        else
            // we can use a background worker to periodically
            // check these failed services and retry on them to also register them
            // but we don't bother to do that here or in services
            // because retry policies were failed already and this will be
            // a waste of resources.
            adapterResolver!.RegisterFailedAdapter(adapter);
    }
}

await using (var scope = app.Services.CreateAsyncScope())
{
    var service = scope.ServiceProvider.GetService<ITransferService>();
    app.MapPost("/transfer", async (CreateTransferDto input) => await service!.TransferAsync(input)).WithName("Transfer");
}



app.Run();