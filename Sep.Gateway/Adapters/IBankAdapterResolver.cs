namespace Sep.Gateway.Adapters;

public interface IBankAdapterResolver
{
    void RegisterAdapter(string identificationNumber, IBankAdapter adapter);
    void RegisterFailedAdapter(IBankAdapter bankAdapter);
    IBankAdapter GetAdapter(string identificationNumber);
}