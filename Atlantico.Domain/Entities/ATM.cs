using System;
using System.Collections.Generic;

namespace Atlantico.Domain
{
    public class ATM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Actve { get; set; }
        public List<ATMBankNote> ATMBankNotes { get; set; }
    }
}
