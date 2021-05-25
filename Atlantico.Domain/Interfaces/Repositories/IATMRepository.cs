using System.Collections.Generic;

namespace Atlantico.Domain.Interfaces.Repositories
{
    public interface IATMRepository
    {
        ATM GetById(int id);
        List<ATM> GetAllActve();

        bool turnOff(int id);
    }
}