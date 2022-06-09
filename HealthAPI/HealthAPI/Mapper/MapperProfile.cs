using AutoMapper;
using HealthAPI.Data.Model;
using HealthAPI.DTOs;

namespace HealthAPI
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<Patient, PatientDTO>().ReverseMap();
            CreateMap<Encounter, EncounterDTO>().ReverseMap();
        }
    }
}
