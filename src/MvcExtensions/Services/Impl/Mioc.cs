using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using am = AutoMapper;
using MvcExtensions.Controller;
using Castle.Windsor;
using Castle.Core;
using AutoMapper.Mappers;


namespace MvcExtensions.Services.Impl
{
    public abstract class Mioc : IOC, IMioc 
    {

        protected am.Configuration MapperCfg { get; private set; }
        protected am.IMappingEngine Mapper { get; private set; }

        public Mioc(IIOC parent)
        {
           MapperCfg = new AutoMapper.Configuration(
               new AutoMapper.TypeMapFactory()
               ,MapperRegistry.AllMappers());
           Initialize();
           Mapper = new AutoMapper.MappingEngine(MapperCfg);
           Kernel.AddComponentInstance<IMioc>(this);
           var p = (WindsorContainer)parent;
           p.AddChildContainer(this);
        }

        #region IMoic Members

        public T Map<S, T>(S source)
        {
            return Mapper.Map<S, T>(source);
        }

        public T Map<S1, S2, T>(S1 source1, S2 source2)
        {
            return Map(source2, Mapper.Map<S1, T>(source1));
        }

        public T Map<S1, S2, S3, T>(S1 source1, S2 source2, S3 source3)
        {
            return Map(source2, source3, Mapper.Map<S1, T>(source1));
        }

        public T Map<S, T>(S source, T target)
        {
            return Mapper.Map<S, T>(source, target);
        }

        public T Map<S1, S2, T>(S1 source1, S2 source2, T Target)
        {
            return Mapper.Map(source1, Mapper.Map(source2, Target));
        }

        public T Map<S1, S2, S3, T>(S1 source1, S2 source2, S3 source3, T Target)
        {
            return Map(source1, source2, Mapper.Map(source3, Target));
        }

        #endregion


        public Wrapper<VType> To<VType>()
        {
            return new Wrapper<VType>() { Mapper = this };
        }
        

        public virtual void Initialize()
        {
        }


        #region IMoic Members

        private CNothing nothing = new CNothing();

        public CNothing Nothing
        {
            get { return nothing; }
        }

        #endregion
    }
}
