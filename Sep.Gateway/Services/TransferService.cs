using Sep.Gateway.Adapters;

namespace Sep.Gateway.Services;

public class TransferService : ITransferService
{
    private readonly IBankAdapterResolver _bankAdapterResolver;

    public TransferService(IBankAdapterResolver bankAdapterResolver)
    {
        _bankAdapterResolver = bankAdapterResolver;
    }


    public async Task<BaseResponse<TransferDto>> TransferAsync(CreateTransferDto input)
    {
        string identificationNumber = input.SourceCard.Substring(0, 6);
        try
        {
            var adapter = _bankAdapterResolver.GetAdapter(identificationNumber);
            var adapterResponse = await adapter.TransferAsync(new TransferMoney(input.SourceCard, input.DestinationCard, input.Password, input.Cvv2,
                input.ExpirationDate, input.PhoneNumber));

            if (!adapterResponse.IsSuccess)
                return new BaseResponse<TransferDto>("Failed", false, null);

            return new BaseResponse<TransferDto>("Success", true,
                new TransferDto(adapterResponse.Data.TransactionId, adapterResponse.Data.ReferenceCode));
        }
        catch (KeyNotFoundException e)
        {
            return new BaseResponse<TransferDto>("No payment provider found for your card", false, null);
        }
    }

}