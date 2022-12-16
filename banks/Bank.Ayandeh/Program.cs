var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string BankIdentificationNumber = "636214";

app.MapGet("/get-identification-number", () => new BaseResponseDto<string>("Success", 0, BankIdentificationNumber))
.WithName("GetIdentificationNumber");

app.MapPost("/transfer", (TransferInput input) =>
{
    TransferResponseDto dto = new TransferResponseDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
    return new BaseResponseDto<TransferResponseDto>("Success", 0, dto);
}).WithName("Transfer");

app.Run();

internal record TransferInput(string SourceCard, string DestinationCard, string Password, string Cvv2, DateTime ExpirationDate, string PhoneNumber);

internal record BaseResponseDto<T>(string Message, int Status, T Data);

internal record TransferResponseDto(string TransactionId, string ReferenceCode);