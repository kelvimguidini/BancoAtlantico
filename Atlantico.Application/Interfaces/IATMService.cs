using Atlantico.Application.DTO;
using System.Collections.Generic;

namespace Atlantico.Application.Interfaces
{
    public interface IATMService
    {
        List<ResponseDTO> Withdraw(WithdrawDTO withdraw);
        List<ResponseDTO> CountNotes(int id);
        bool turnOff(int id);
        List<ATMResponseDTO> GetActveATM();
    }
}