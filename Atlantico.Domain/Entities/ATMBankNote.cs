using System;

namespace Atlantico.Domain
{
    public class ATMBankNote
    {
        public int Id { get; set; }
        public BankNoteType BankNote { get; set; }
        public ATM ATM { get; set; }
        public int Count { get; set; }
    }
}