using System.ComponentModel.DataAnnotations;

namespace Atlantico.Application.DTO
{
    public class WithdrawDTO
    {
        [Required(ErrorMessage = "Id do caixa eletrônico é obrigatório.")]
        public int ATMId { get; set; }

        [Required(ErrorMessage = "Valor do saque é obrigatório.")]
        public int Value { get; set; }
    }
}