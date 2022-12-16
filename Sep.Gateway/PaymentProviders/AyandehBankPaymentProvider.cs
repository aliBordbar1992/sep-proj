using System.Text.Json;
using Polly;
using Polly.Timeout;

namespace Sep.Gateway.PaymentProviders;

public class AyandehBankPaymentProvider : IAyandehBankPaymentProvider
{
    private readonly HttpClient _client;

    public AyandehBankPaymentProvider(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
    }

    public async Task<BaseResponseDto<string>> GetBankIdentificationNumberAsync()
    {
        var timeout = Policy.TimeoutAsync(TimeSpan.FromSeconds(2), TimeoutStrategy.Pessimistic);
        var retry = Policy<BaseResponseDto<string>>.Handle<Exception>().RetryAsync(2);
        var fallback = Policy<BaseResponseDto<string>>.Handle<Exception>().FallbackAsync(new BaseResponseDto<string>("", -99, ""));

        var r = await fallback
            .WrapAsync(retry)
            .WrapAsync(timeout)
            .ExecuteAsync(async () =>
                (await _client.GetFromJsonAsync<BaseResponseDto<string>>(ApiUrl.AyandeIdentificationNumber))!);

        return r;
    }

    public async Task<BaseResponseDto<TransferResponseDto>> TransferAsync(TransferDto transfer)
    {
        var response = await _client.PostAsJsonAsync(ApiUrl.AyandeTransfer, transfer);
        return JsonSerializer.Deserialize<BaseResponseDto<TransferResponseDto>>(await response.Content.ReadAsStringAsync())!;
    }
}