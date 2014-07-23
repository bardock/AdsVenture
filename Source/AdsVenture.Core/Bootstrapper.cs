using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using AdsVenture.Commons.Helpers;

namespace AdsVenture.Core
{
    public class Bootstrapper
    {
        private static bool _isInitialized = false;

        public static bool IsInitialized
        {
            get { return _isInitialized; }
        }

        private static IServiceLocator _serviceLocator;

        public static Data.CacheContext Cache { get { return _serviceLocator.GetService<Data.CacheContext>(); } }

        public static void Init(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            Managers._CacheManager.Instance.BindEvents();

            InitValidation();

            InitMappings();

            Bardock.Utils.Globalization.Resources.Register(new Bardock.Utils.Globalization.TypedClassResourceProvider(typeof(Commons.Entities.Resources.Entities)));

            _isInitialized = true;
        }

        private static void InitValidation()
        {
            ValidatorOptions.PropertyNameResolver = Validation.ValidationResolvers.DisplayNameResolver;
            ValidatorOptions.DisplayNameResolver = Validation.ValidationResolvers.DisplayNameResolver;
        }


        private static void InitMappings()
        {
            AutoMapper.Mapper.CreateMap<DTO.AdvertiserCreate, Commons.Entities.Advertiser>().ReverseMap();
            AutoMapper.Mapper.CreateMap<DTO.AdvertiserUpdate, Commons.Entities.Advertiser>().ReverseMap();
            AutoMapper.Mapper.CreateMap<DTO.SlotEvent, Commons.Entities.SlotEvent>();
            AutoMapper.Mapper.CreateMap<DTO.SlotEventTarget, Commons.Entities.SlotEventTarget>();
        }

    }
}
