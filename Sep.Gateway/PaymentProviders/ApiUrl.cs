namespace Sep.Gateway.PaymentProviders;

public static class ApiUrl
{
    public const string Mellat = "https://localhost:7003";
    public const string MellatIdentificationNumber = $"{Mellat}/get-identification-number";
    public const string MellatTransfer = $"{Mellat}/get-identification-number";

    public const string Saman = "https://localhost:7005";
    public const string SamanIdentificationNumber = $"{Saman}/get-identification-number";
    public const string SamanTransfer = $"{Saman}/get-identification-number";

    public const string Ayande = "https://localhost:7001";
    public const string AyandeIdentificationNumber = $"{Ayande}/get-identification-number";
    public const string AyandeTransfer = $"{Ayande}/get-identification-number";
}