using System.Text.Json;
using Polly;
using Polly.Timeout;

namespace Sep.Gateway.PaymentProviders;

public class SamanBankPaymentProvider : ISamanBankPaymentProvider
{
    private readonly HttpClient _client;

    public SamanBankPaymentProvider(IHttpClientFactory httpClientFactory)
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
                (await _client.GetFromJsonAsync<BaseResponseDto<string>>(ApiUrl.SamanIdentificationNumber))!);

        return r;
    }

    public async Task<BaseResponseDto<TransferResponseDto>> TransferAsync(TransferDto transfer)
    {
        var response = await _client.PostAsJsonAsync(ApiUrl.SamanTransfer, transfer);
        return JsonSerializer.Deserialize<BaseResponseDto<TransferResponseDto>>(await response.Content.ReadAsStringAsync())!;
    }
}