namespace Sep.Gateway.Services;

public record CreateTransferDto(string SourceCard, string DestinationCard, string Password, string Cvv2, DateTime ExpirationDate, string PhoneNumber);