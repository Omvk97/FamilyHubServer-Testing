namespace API.Helpers.Hashing
{
    public interface IHashing
    {
        string Hash(string password);
        bool Check(string hashedPassword, string inputPassword);
    }
}
