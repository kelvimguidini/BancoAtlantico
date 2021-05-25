using Atlantico.Application.DTO;
using Atlantico.Application.Services;
using Atlantico.CrossCutting.Massages.Interfaces;
using Atlantico.Data.Context;
using Atlantico.Data.Repositories;
using Atlantico.Domain;
using Atlantico.Domain.Interfaces.Repositories;
using AutoMapper;
using KissLog;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using Xunit;

namespace Atlantico.Application.Tests
{
    public class ATMServiceTest
    {
        private ATMService prvGetMockATMService()
        {

            #region popular banco inMemory

            var mockContext = new Mock<ContextDB>();
            var options = new DbContextOptionsBuilder<ContextDB>()
               .UseInMemoryDatabase(databaseName: "Test").EnableSensitiveDataLogging()
               .Options;

            var context = new ContextDB(options);

            var atm = context.ATM.Add(new ATM
            {
                Actve = true,
                Name = "Sede"
            });

            var atmFarmacia = context.ATM.Add(new ATM
            {
                Actve = true,
                Name = "Farmácia X"
            });

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


            context.ATMBankNote.Add(new ATMBankNote
            {
                ATM = atmFarmacia.Entity,
                BankNote = BankNoteType.Fifty,
                Count = 50
            });
            context.ATMBankNote.Add(new ATMBankNote
            {
                ATM = atmFarmacia.Entity,
                BankNote = BankNoteType.Twenty,
                Count = 50
            });
            context.ATMBankNote.Add(new ATMBankNote
            {
                ATM = atmFarmacia.Entity,
                BankNote = BankNoteType.Ten,
                Count = 50
            });
            context.ATMBankNote.Add(new ATMBankNote
            {
                ATM = atmFarmacia.Entity,
                BankNote = BankNoteType.Five,
                Count = 50
            });
            context.ATMBankNote.Add(new ATMBankNote
            {
                ATM = atmFarmacia.Entity,
                BankNote = BankNoteType.Two,
                Count = 50
            });

            context.SaveChanges();

            #endregion

            var mockATMRepository = new ATMRepository(context);

            var mockATMBankNoteRepository = new ATMBankNoteRepository(context);
            Mock<INotificator> mockNotificator = new Mock<INotificator>();
            Mock<ILogger> mockLogger = new Mock<ILogger>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();

            return new ATMService(mockATMRepository, mockATMBankNoteRepository, mockNotificator.Object, mockLogger.Object, mockMapper.Object);
        }

        private readonly ATMService _atm;

        public ATMServiceTest()
        {
            _atm = prvGetMockATMService();
        }

        [Fact]
        public void ATMDoesNotExist()
        {
            // Arrange
            var result = _atm.Withdraw(new WithdrawDTO
            {
                ATMId = 10,
                Value = 500
            });

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void ValueNotAuthorized()
        {
            // Arrange
            var result = _atm.Withdraw(new WithdrawDTO
            {
                ATMId = 1,
                Value = 10001
            });

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void WithdrawSucess()
        {
            // Arrange
            var result = _atm.Withdraw(new WithdrawDTO
            {
                ATMId = 2,
                Value = 1000
            });

            // Assert
            Assert.True(result.Where(x => x.BankNote == BankNoteType.Fifty).Sum(x => x.Count) == 20
                && result.Where(x => x.BankNote == BankNoteType.Twenty).Sum(x => x.Count) == 0
                && result.Where(x => x.BankNote == BankNoteType.Ten).Sum(x => x.Count) == 0
                && result.Where(x => x.BankNote == BankNoteType.Five).Sum(x => x.Count) == 0
                && result.Where(x => x.BankNote == BankNoteType.Two).Sum(x => x.Count) == 0
            );
        }

        [Fact]
        public void WithdrawSucessKeepinNotes()
        {
            // Arrange
            var result = _atm.Withdraw(new WithdrawDTO
            {
                ATMId = 1,
                Value = 224
            });

            // Assert
            Assert.True(result.Where(x => x.BankNote == BankNoteType.Fifty).Sum(x => x.Count) == 3
                && result.Where(x => x.BankNote == BankNoteType.Twenty).Sum(x => x.Count) == 2
                && result.Where(x => x.BankNote == BankNoteType.Ten).Sum(x => x.Count) == 2
                && result.Where(x => x.BankNote == BankNoteType.Five).Sum(x => x.Count) == 2
                && result.Where(x => x.BankNote == BankNoteType.Two).Sum(x => x.Count) == 2
            );
        }

        [Fact]
        public void ValueNotMultipleLess5()
        {
            // Arrange
            var result = _atm.Withdraw(new WithdrawDTO
            {
                ATMId = 1,
                Value = 3
            });

            // Assert
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void ValueNotMultiple()
        {
            // Arrange
            var result = _atm.Withdraw(new WithdrawDTO
            {
                ATMId = 2,
                Value = 51
            });

            // Assert
            Assert.True(result.Where(x => x.BankNote == BankNoteType.Fifty).Sum(x => x.Count) == 0
                && result.Where(x => x.BankNote == BankNoteType.Twenty).Sum(x => x.Count) == 2
                && result.Where(x => x.BankNote == BankNoteType.Ten).Sum(x => x.Count) == 0
                && result.Where(x => x.BankNote == BankNoteType.Five).Sum(x => x.Count) == 1
                && result.Where(x => x.BankNote == BankNoteType.Two).Sum(x => x.Count) == 3
            );
        }
    }
}