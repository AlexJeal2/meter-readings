using AutoMapper;
using MeterReadings.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadings.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapsCreateAccountModelToAccountModel();
            MapsMeterReadingDTOToMeterReading();
        }

        public void MapsCreateAccountModelToAccountModel()
        {
            CreateMap<AccountDTO, Account>();
            CreateMap<Account, AccountDTO>();
        }

        public void MapsMeterReadingDTOToMeterReading()
        {
            CreateMap<MeterReadingDTO, MeterReading>();
            //Don't map the account, we can't
            //.ForMember(dst => dst.Account, opt => opt.Ignore());

            CreateMap<MeterReading, MeterReadingDTO>();
                //.ForMember(dst => dst.ValidationErrors, opt => opt.Ignore());
        }
    }
}
