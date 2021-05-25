namespace Atlantico.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
    }
}