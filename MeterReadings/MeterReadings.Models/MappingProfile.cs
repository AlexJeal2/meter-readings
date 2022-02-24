using AutoMapper;
using MeterReadings.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadings.Models
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapsMeterReadingDTOToMeterReading();
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
