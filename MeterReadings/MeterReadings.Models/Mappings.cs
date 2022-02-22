using AutoMapper;
using MeterReadings.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadings.Models
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            MapsCreateAccountModelToAccountModel();
        }

        public void MapsCreateAccountModelToAccountModel()
        {
            CreateMap<AccountCreateModel, Account>();
        }
    }
}
