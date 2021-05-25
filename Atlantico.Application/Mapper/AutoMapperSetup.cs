using Atlantico.Application.DTO;
using Atlantico.Domain;
using AutoMapper;
using System;

namespace Atlantico.Application.Mapper
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup()
        {
            #region Entity To DTO

            CreateMap<ATMBankNote, ResponseDTO>()
                .ForMember(a => a.BankNote, b => b.MapFrom(c => c.BankNote))
                .ForMember(a => a.Count, b => b.MapFrom(c => c.Count))
                ;

            CreateMap<ATM, ATMResponseDTO>();

            #endregion
        }
    }
}