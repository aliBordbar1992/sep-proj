using Sep.Gateway.PaymentProviders;

namespace Sep.Gateway.Adapters;

public class AyandehBankAdapter : IBankAdapter
{
    private readonly IAyandehBankPaymentProvider _provider;

    public AyandehBankAdapter(IAyandehBankPaymentProvider provider)
    {
        _provider = provider;
    }

    public async Task<ApiResponse<string>> GetIdentificationNumberAsync()
    {
        try
        {
            var apiResponse = await _provider.GetBankIdentificationNumberAsync();
            if (apiResponse.Status == 0)
                return ApiResponse<string>.Success(apiResponse.Data);

            return new ApiResponse<string>("Api call failed", -1, "");
        }
        catch (Exception e)
        {
            return new ApiResponse<string>("Unable to get identification number", -1, "");
        }
    }

    public async Task<ApiResponse<TransferResult>> TransferAsync(TransferMoney transferRequest)
    {
        var r = MapToTransferDto(transferRequest);
        var result = await _provider.TransferAsync(r);
        if (result.Status == 0)
        {
            var response = MapToTransferResult(result.Data);
            return ApiResponse<TransferResult>.Success(response);
        }

        return new ApiResponse<TransferResult>(result.Message, result.Status, null);
    }

    private TransferDto MapToTransferDto(TransferMoney input) => new(input.SourceCard,
        input.DestinationCard, input.Password, input.Cvv2, input.ExpirationDate, input.PhoneNumber);

    private TransferResult MapToTransferResult(TransferResponseDto input) =>
        new(input.TransactionId, input.ReferenceCode);
}