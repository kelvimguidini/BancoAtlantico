using Atlantico.Application.DTO;

namespace Atlantico.Application.Interfaces
{
    public interface IUserService
    {
        string Authenticate(LoginDTO login);
    }
}