// these interfaces represent each provider api
// the methods in these interfaces might be different
// but for the sake of simplicity we use the same end-points
// and inputs

namespace Sep.Gateway.PaymentProviders;

public interface IMellatBankPaymentProvider
{
    Task<BaseResponseDto<string>> GetBankIdentificationNumberAsync();
    Task<BaseResponseDto<TransferResponseDto>> TransferAsync(TransferDto transfer);
}

public interface ISamanBankPaymentProvider
{
    Task<BaseResponseDto<string>> GetBankIdentificationNumberAsync();
    Task<BaseResponseDto<TransferResponseDto>> TransferAsync(TransferDto transfer);
}

public interface IAyandehBankPaymentProvider
{
    Task<BaseResponseDto<string>> GetBankIdentificationNumberAsync();
    Task<BaseResponseDto<TransferResponseDto>> TransferAsync(TransferDto transfer);
}

public record TransferDto(string SourceCard, string DestinationCard, string Password, string Cvv2,
    DateTime ExpirationDate, string PhoneNumber);

public record BaseResponseDto<T>(string Message, int Status, T Data);

public record TransferResponseDto(string TransactionId, string ReferenceCode);