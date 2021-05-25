using Atlantico.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atlantico.Application.DTO
{
    public class ATMResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Actve { get; set; }
        public List<ResponseDTO> ATMBankNotes { get; set; }
    }
}