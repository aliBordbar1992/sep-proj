namespace Sep.Gateway.Adapters;

public interface IBankAdapter
{
    Task<ApiResponse<string>> GetIdentificationNumberAsync();
    Task<ApiResponse<TransferResult>> TransferAsync(TransferMoney transferRequest);
}