using System.Text.Json;
using Polly;
using Polly.Timeout;

namespace Sep.Gateway.PaymentProviders;

public class MellatBankPaymentProvider : IMellatBankPaymentProvider
{
    private readonly HttpClient _client;

    public MellatBankPaymentProvider(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
    }

    public async Task<BaseResponseDto<string>> GetBankIdentificationNumberAsync()
    {
        // we can define multiple policies to deal with different situations
        // here we simply defined a timeout policy, a fallback and a retry with delay
        // we can also define a circuit breaker (singleton) to deal with down times of dependent
        // services.
        // mellat bank provider simulates a simple delay in response to test timeout and retry
        // functionalities

        var timeout = Policy.TimeoutAsync(TimeSpan.FromSeconds(2), TimeoutStrategy.Pessimistic);
        var retry = Policy<BaseResponseDto<string>>.Handle<Exception>().RetryAsync(2);
        var fallback = Policy<BaseResponseDto<string>>.Handle<Exception>().FallbackAsync(new BaseResponseDto<string>("", -99, ""));

        var r = await fallback
            .WrapAsync(retry)
            .WrapAsync(timeout)
            .ExecuteAsync(async () =>
            (await _client.GetFromJsonAsync<BaseResponseDto<string>>(ApiUrl.MellatIdentificationNumber))!);

        return r;
    }

    public async Task<BaseResponseDto<TransferResponseDto>> TransferAsync(TransferDto transfer)
    {
        var response = await _client.PostAsJsonAsync(ApiUrl.MellatTransfer, transfer);
        return JsonSerializer.Deserialize<BaseResponseDto<TransferResponseDto>>(await response.Content.ReadAsStringAsync())!;
    }
}