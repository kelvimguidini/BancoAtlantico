using Atlantico.Data.Context;
using Atlantico.Domain;
using Atlantico.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantico.Data.Repositories
{
    public class ATMBankNoteRepository : IATMBankNoteRepository
    {
        private readonly ContextDB _db;

        public ATMBankNoteRepository(ContextDB contextDb)
        {
            _db = contextDb;
        }

        public void Update(ATMBankNote bankNote)
        {
            try
            {
                var f = _db.ATMBankNote.Find(bankNote.Id);

                f.Count = bankNote.Count;
                _db.Update(f);

                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar quantidade de notas disponíveis: " + ex.Message);
            }
        }
    }
}