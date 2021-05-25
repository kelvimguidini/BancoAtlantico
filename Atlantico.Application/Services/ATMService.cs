using Atlantico.Application.DTO;
using Atlantico.Application.Interfaces;
using Atlantico.CrossCutting.Massages.Interfaces;
using Atlantico.Domain;
using Atlantico.Domain.Interfaces.Repositories;
using AutoMapper;
using KissLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantico.Application.Services
{
    public class ATMService : IATMService
    {
        private readonly IATMRepository _atmRepository;
        private readonly IATMBankNoteRepository _atmBankNoteRepository;
        private readonly INotificator _notificator;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        private readonly int CRITICALVALUE = 5;

        public ATMService(IATMRepository atmRepository,
            IATMBankNoteRepository atmBankNoteRepository,
            INotificator notificator,
            ILogger logger,
            IMapper mapper)
        {
            _atmRepository = atmRepository;
            _notificator = notificator;
            _atmBankNoteRepository = atmBankNoteRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public List<ResponseDTO> Withdraw(WithdrawDTO withdraw)
        {
            try
            {
                var atm = _atmRepository.GetById(withdraw.ATMId);

                int iValue, iBankNote50, iBankNote20, iBankNote10, iBankNote5, iBankNote2, i;
                iBankNote50 = iBankNote20 = iBankNote10 = iBankNote5 = iBankNote2 = i = 0;
                
                if(atm == null)
                {
                    _notificator.notify("Caixa eletrônico não encontrado");
                    return new List<ResponseDTO>();
                }
                if(!atm.Actve)
                {
                    _notificator.notify("Caixa eletrônico desativado");
                    return new List<ResponseDTO>();
                }

                if ((withdraw.Value % 2) == 1 && withdraw.Value < 5)
                {
                    string notes = string.Empty;
                    foreach (var bankNote in atm.ATMBankNotes.Where(x => x.Count > 0))
                    {
                        if (!string.IsNullOrEmpty(notes))
                        {
                            notes += "- ";
                        }
                        notes += ((int)bankNote.BankNote).ToString() + ",00";

                    }

                    _notificator.notify("Valor não permitido. Cédulas disponíveis: " + notes);
                    return new List<ResponseDTO>();
                }

                if (withdraw.Value <= 0 || withdraw.Value > 10000)
                {
                    _notificator.notify("Valor não permitido. Valor do saque deve ser maior que 0 e menor igual a 10000");
                    return new List<ResponseDTO>();
                }

                if (withdraw.Value > atm.ATMBankNotes.Sum(x => (int)x.BankNote * x.Count))
                {
                    _notificator.notify("Este caixa eletrônico não possui este valor");
                    return new List<ResponseDTO>();
                }

                #region Calcula qtd notas

                var fifty = atm.ATMBankNotes.FirstOrDefault(x => x.BankNote == BankNoteType.Fifty);
                var twenty = atm.ATMBankNotes.FirstOrDefault(x => x.BankNote == BankNoteType.Twenty);
                var ten = atm.ATMBankNotes.FirstOrDefault(x => x.BankNote == BankNoteType.Ten);
                var five = atm.ATMBankNotes.FirstOrDefault(x => x.BankNote == BankNoteType.Five);
                var two = atm.ATMBankNotes.FirstOrDefault(x => x.BankNote == BankNoteType.Two);

                iValue = withdraw.Value;
                bool noKeep = false;
                while (iValue> 0)
                {
                    if (five.Count > 1 && iValue >= 5 && (iValue % 2) == 1)
                    {
                        i = 1;
                       
                        iValue -= 5 * i;
                        five.Count -= i;
                        iBankNote5 += i;
                    }

                    if (iValue >= 50)
                    {
                        i = iValue / 50;
                        if (fifty != null && CRITICALVALUE >= fifty.Count - i)
                        {
                            i = calculate(50, iValue, fifty.Count, noKeep);
                        }
                        iValue -= 50 * i;
                        fifty.Count -= i;
                        iBankNote50 += i;
                    }

                    if (iValue >= 20)
                    {
                        i = iValue / 20;
                        if (twenty != null && CRITICALVALUE >= twenty.Count - i)
                        {
                            i = calculate(20, iValue, twenty.Count, noKeep);
                        }
                        iValue -= 20 * i;
                        twenty.Count -= i;
                        iBankNote20 += i;
                    }

                    if (iValue >= 10)
                    {
                        i = iValue / 10;
                        if (ten != null && CRITICALVALUE >= ten.Count - i)
                        {
                            i = calculate(10, iValue, ten.Count, noKeep);
                        }
                        iValue -= 10 * i;
                        ten.Count -= i;
                        iBankNote10 += i;
                    }

                    if (iValue >= 5 && ((iValue % 5) % 2) == 0)
                    {
                        i = iValue / 5;
                        if (five != null && CRITICALVALUE >= five.Count - i)
                        {
                            i = calculate(5, iValue, five.Count, noKeep);
                        }
                        iValue -= 5 * i;
                        five.Count -= i;
                        iBankNote5 += i;
                    }

                    if (iValue >= 2)
                    {
                        i = iValue / 2;
                        if (two != null && CRITICALVALUE >= two.Count - i)
                        {
                            i = calculate(2, iValue, two.Count, noKeep);
                        }
                        iValue -= 2 * i;
                        two.Count -= i;
                        iBankNote2 += i;
                    }
                    noKeep = true;
                }
                #endregion

                #region MontaRetorno
                List<ResponseDTO> result = new List<ResponseDTO>();

                result.Add(new ResponseDTO
                {
                    BankNote = BankNoteType.Fifty,
                    Count = iBankNote50
                });
                result.Add(new ResponseDTO
                {
                    BankNote = BankNoteType.Twenty,
                    Count = iBankNote20
                });
                result.Add(new ResponseDTO
                {
                    BankNote = BankNoteType.Ten,
                    Count = iBankNote10
                });
                result.Add(new ResponseDTO
                {
                    BankNote = BankNoteType.Five,
                    Count = iBankNote5
                });
                result.Add(new ResponseDTO
                {
                    BankNote = BankNoteType.Two,
                    Count = iBankNote2
                });

                #endregion

                #region Atualiza quantidade de notas


                _atmBankNoteRepository.Update(fifty);

                _atmBankNoteRepository.Update(twenty);

                _atmBankNoteRepository.Update(ten);

                _atmBankNoteRepository.Update(five);

                _atmBankNoteRepository.Update(two);

                #endregion


                return result;
            }
            catch (Exception ex)
            {
                _logger.Log(KissLog.LogLevel.Error, ex);
                throw new Exception("Erro ao calcular saque: " + ex.Message);
            }
        }

        private int calculate(int bankNote, int value, int totalNotes, bool noKeep)
        {
            int available = 0;
            if (!noKeep)
            {
                available = totalNotes - CRITICALVALUE > 0 ? totalNotes - CRITICALVALUE : 0;
            }
            else
            {
                available = totalNotes > 0 ? totalNotes : 0;
            }

            return value / bankNote <= available ? value / bankNote : available;
        }

        public List<ResponseDTO> CountNotes(int id)
        {
            var atm = _atmRepository.GetById(id);

            List<ResponseDTO> result = _mapper.Map<List<ResponseDTO>>(atm.ATMBankNotes);
            return result;
        }

        public List<ATMResponseDTO> GetActveATM()
        {
            var atms = _atmRepository.GetAllActve();

            List<ATMResponseDTO> result = _mapper.Map<List<ATMResponseDTO>>(atms);
            return result;

        }

        public bool turnOff(int id)
        {
            return _atmRepository.turnOff(id);
        }

    }
}