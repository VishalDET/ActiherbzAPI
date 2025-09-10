namespace SynfoShopAPI.Models
{
    public interface IJwtAuth
    {
        string Authentication(string username, string password);
    }
}
