using System;
using System.Globalization;
using AutoMapper;
using PuppeteerSharp;
using SomNet.Models;

namespace SomNet.Utilities.Mappers
{

    // Map object attributes automatically - uses the AutoMapper package
    public class DtoMapping : Profile
    {
        public Sta70DataDTO Mapping(SomVector data)
        {
            // Get current time
            DateTime localDate = DateTime.Now;
            DateTime utcDate = DateTime.UtcNow;
            var region = new CultureInfo("en-GB");

            //Step1: Initialize the Mapper
            var mapper = InitializeAutomapper();

            //Step2: Map the OrderRequest object to Order DTO
            var DTOData = mapper.Map<SomVector, Sta70DataDTO>(data);

            return DTOData;
        }

        static Mapper InitializeAutomapper()
        {
            //SoMVector => Sta70DataDTO - double => datatype
            var Vector70Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SomVector, Sta70DataDTO>()
                    //.IgnoreAllSourcePropertiesWithAnInaccessibleSetter().IgnoreAllPropertiesWithAnInaccessibleSetter()
                    .ForPath(dest => dest.OverAllResult, action => action.MapFrom(src => src.ElementAt(0)))
                    .ForPath(dest => dest.CycleTime, action => action.MapFrom(src => src.ElementAt(1)))
                    .ForPath(dest => dest.CapnutTypeResult, action => action.MapFrom(src => src.ElementAt(2)))
                    .ForPath(dest => dest.OMVSpringResult, action => action.MapFrom(src => src.ElementAt(3)))
                    .ForPath(dest => dest.NozzlePreLoadForce, action => action.MapFrom(src => src.ElementAt(4)))
                    .ForPath(dest => dest.NozzlePreLoadPosition, action => action.MapFrom(src => src.ElementAt(5)))
                    .ForPath(dest => dest.StackBuildResult, action => action.MapFrom(src => src.ElementAt(6)))
                    .ForPath(dest => dest.CapnutTorque, action => action.MapFrom(src => src.ElementAt(7)))
                    .ForPath(dest => dest.CapnutTorqueAngle, action => action.MapFrom(src => src.ElementAt(8)))
                    .ForPath(dest => dest.CapnutFinalAngle, action => action.MapFrom(src => src.ElementAt(9)))
                    .ForPath(dest => dest.CapnutAssemblyResult, action => action.MapFrom(src => src.ElementAt(10)))
                    .ForPath(dest => dest.Id, action => action.MapFrom(src => src.Id))
                    .ForPath(dest => dest.Label, action => action.MapFrom(src => src.Label))
                    .ForPath(dest => dest.Timestamp, action => action.MapFrom(src => src.Timestamp))
                    .ReverseMap();
                //.ForPath(dest => dest.ElementAt(0), action => action.MapFrom(src => src.OverAllResult))
                //.ForPath(dest => dest.ElementAt(1), action => action.MapFrom(src => src.CycleTime))
                //.ForPath(dest => dest.ElementAt(2), action => action.MapFrom(src => src.CapNutTypeResult))
                //.ForPath(dest => dest.ElementAt(3), action => action.MapFrom(src => src.OMVSpringResult_PI))
                //.ForPath(dest => dest.ElementAt(4), action => action.MapFrom(src => src.NozzlePreLoadForce))
                //.ForPath(dest => dest.ElementAt(5), action => action.MapFrom(src => src.NozzlePreLoadPosition))
                //.ForPath(dest => dest.ElementAt(6), action => action.MapFrom(src => src.StackBuildResult))
                //.ForPath(dest => dest.ElementAt(7), action => action.MapFrom(src => src.CapNutTorque))
                //.ForPath(dest => dest.ElementAt(8), action => action.MapFrom(src => src.CapNutTorqueAngle))
                //.ForPath(dest => dest.ElementAt(9), action => action.MapFrom(src => src.CapNutFinalAngle))
                //.ForPath(dest => dest.ElementAt(10), action => action.MapFrom(src => src.CapNutAssemblyResult))
                //.ForPath(dest => dest.Label, action => action.MapFrom(src => src.Label))
                //.ForPath(dest => dest.TimeStamp, action => action.MapFrom(src => src.TimeStamp));
            });

            var mapper = new Mapper(Vector70Config);
            return mapper;
        }
    }
}