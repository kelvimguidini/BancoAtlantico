using Atlantico.Data.Context;
using Atlantico.Domain;
using Atlantico.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantico.Data.Repositories
{
    public class ATMRepository : IATMRepository
    {
        private readonly ContextDB _db;

        public ATMRepository(ContextDB contextDb)
        {
            _db = contextDb;
        }

        public ATM GetById(int id)
        {
            try
            {
                return _db.ATM.Include(q => q.ATMBankNotes).AsNoTracking().Where(q => q.Id == id && q.Actve).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter caixa eletrônico: " + ex.Message);
            }
        }
        
        public List<ATM> GetAllActve()
        {
            try
            {
                return _db.ATM.Include(q => q.ATMBankNotes).AsNoTracking().Where(q => q.Actve).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter caixas eletrônicos: " + ex.Message);
            }
        }

        public bool turnOff(int id)
        {
            try
            {
                var atm = _db.ATM.Where(q => q.Id == id).FirstOrDefault();
                atm.Actve = false;
                _db.Update(atm);

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desativar caixa: " + ex.Message);
            }
        }
    }
}