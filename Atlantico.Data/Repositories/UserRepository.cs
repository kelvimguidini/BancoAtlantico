using Atlantico.Data.Context;
using Atlantico.Domain;
using Atlantico.Domain.Interfaces.Repositories;
using System;
using System.Linq;

namespace Atlantico.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ContextDB _db;

        public UserRepository(ContextDB contextDb)
        {
            _db = contextDb;
        }

        public User GetByUsername(string username)
        {
            try
            {
                return _db.User.Where(q => q.Username == username).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter usuário: " + ex.Message);
            }

        }
    }
}