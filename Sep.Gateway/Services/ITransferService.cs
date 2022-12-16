namespace Sep.Gateway.Services;

public interface ITransferService
{
    Task<BaseResponse<TransferDto>> TransferAsync(CreateTransferDto input);
}