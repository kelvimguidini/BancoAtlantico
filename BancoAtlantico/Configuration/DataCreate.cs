using Atlantico.CrossCutting.Utils;
using Atlantico.Data.Context;
using Atlantico.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlantico.WebApi.Configuration
{
    public static class DataCreate
    {
        public static void createDataInMemory()
        {

            var options = new DbContextOptionsBuilder<ContextDB>()
              .UseInMemoryDatabase(databaseName: "Test")
              .Options;
            using (var context = new ContextDB(options))
            {
                var user = new User
                {
                    Username = "admin",
                    Password = Hash.GenerateHashMD5("admin"),
                    Active = true
                };

                context.User.Add(user);

                var atm = context.ATM.Add(new ATM
                {
                    Actve = true,
                    Name = "Sede"
                });


                var atmXpto = context.ATM.Add(new ATM
                {
                    Actve = true,
                    Name = "Caixa da rua XPTO"
                });

                #region caixa xpto
                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atmXpto.Entity,
                    BankNote = BankNoteType.Fifty,
                    Count = 3
                });

                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atmXpto.Entity,
                    BankNote = BankNoteType.Twenty,
                    Count = 50
                });
                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atmXpto.Entity,
                    BankNote = BankNoteType.Ten,
                    Count = 25
                });
                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atmXpto.Entity,
                    BankNote = BankNoteType.Five,
                    Count = 2
                });
                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atmXpto.Entity,
                    BankNote = BankNoteType.Two,
                    Count = 60
                });
                #endregion

                #region caixa sede
                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atm.Entity,
                    BankNote = BankNoteType.Fifty,
                    Count = 7
                });

                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atm.Entity,
                    BankNote = BankNoteType.Twenty,
                    Count = 7
                });
                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atm.Entity,
                    BankNote = BankNoteType.Ten,
                    Count = 7
                });
                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atm.Entity,
                    BankNote = BankNoteType.Five,
                    Count = 7
                });
                context.ATMBankNote.Add(new ATMBankNote
                {
                    ATM = atm.Entity,
                    BankNote = BankNoteType.Two,
                    Count = 7
                });
                #endregion
                context.SaveChanges();
            }
        }
    }
}
