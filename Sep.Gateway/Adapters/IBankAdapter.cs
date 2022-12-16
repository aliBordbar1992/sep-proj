namespace Sep.Gateway.Adapters;

public interface IBankAdapter
{
    Task<ApiResponse<string>> GetIdentificationNumberAsync();
    Task<ApiResponse<TransferResult>> TransferAsync(TransferMoney transferRequest);
}

public interface IBankAdapterResolver
{
    void RegisterAdapter(string identificationNumber, IBankAdapter adapter);
    void RegisterFailedAdapter(IBankAdapter bankAdapter);
    IBankAdapter GetAdapter(string identificationNumber);
}

public class BankAdapterResolver : IBankAdapterResolver
{
    private readonly Dictionary<string, IBankAdapter> _bankAdapters = new();
    private readonly List<IBankAdapter> _failedAdapters = new();

    public void RegisterAdapter(string identificationNumber, IBankAdapter adapter)
    {
        _bankAdapters[identificationNumber] = adapter;
    }

    public void RegisterFailedAdapter(IBankAdapter bankAdapter)
    {
        _failedAdapters.Add(bankAdapter);
    }

    public IBankAdapter GetAdapter(string identificationNumber)
    {
        if (!_bankAdapters.ContainsKey(identificationNumber))
            throw new KeyNotFoundException();

        return _bankAdapters[identificationNumber];
    }
}