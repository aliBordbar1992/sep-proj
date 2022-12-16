namespace Sep.Gateway.Adapters;

public record TransferMoney(string SourceCard, string DestinationCard, string Password, string Cvv2, DateTime ExpirationDate, string PhoneNumber);
public record TransferResult(string TransactionId, string ReferenceCode);

public record ApiResponse<T>(string Message, int Status, T Data)
{
    public bool IsSuccess = Status == 0;
    public static ApiResponse<T> Success(T data) => new ApiResponse<T>("Api call succeeded", 0, data);
}