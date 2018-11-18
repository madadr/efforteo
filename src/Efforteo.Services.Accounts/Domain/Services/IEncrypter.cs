namespace Efforteo.Services.Accounts.Domain.Services
{
    public interface IEncrypter
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}