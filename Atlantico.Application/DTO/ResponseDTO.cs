using Atlantico.Domain;
using System.ComponentModel.DataAnnotations;

namespace Atlantico.Application.DTO
{
    public class ResponseDTO
    {
        public BankNoteType BankNote { get; set; }

        public int Count { get; set; }
    }
}