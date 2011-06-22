using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using CodeCovered.GeoShop.Infrastructure.NHibernate;
using NHibernate;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace CodeCovered.GeoShop.Server.Mapping.Tests
{
    [TestFixture]
    public class AutoMappingExamples
    {
        [Test]
        public void BasicUsage()
        {
            Mapper.CreateMap<BasicEntity, BasicDto>();
            Mapper.AssertConfigurationIsValid();

            var entity = new BasicEntity
                        {
                            Id = 1,
                            Name = "fluffy",
                            Time = new DateTime(100000L)
                        };

            var dto = Mapper.Map<BasicEntity, BasicDto>(entity);

            Assert.That(dto.Id, Is.EqualTo(entity.Id));
            Assert.That(dto.name, Is.EqualTo(entity.Name));
            Assert.That(dto.Value, Is.EqualTo(entity.GetValue()));
            Assert.That(dto.TimeTicks, Is.EqualTo(entity.Time.Ticks));
        }

        [Test]
        public void Projections()
        {
            Mapper.CreateMap<ProjectedEntity, ProjectedDto>()
                .ForMember(dest => dest.Day, o => o.MapFrom(src => src.Time.Day))
                .ForMember(dest => dest.Month, o => o.MapFrom(src => src.Time.Month))
                .ForMember(dest => dest.Year, o => o.MapFrom(src => src.Time.Year));

            Mapper.AssertConfigurationIsValid();

            var entity = new ProjectedEntity { Time = new DateTime(1900, 05, 05) };
            var dto = Mapper.Map<ProjectedEntity, ProjectedDto>(entity);

            Assert.That(dto.Day, Is.EqualTo(entity.Time.Day));
            Assert.That(dto.Month, Is.EqualTo(entity.Time.Month));
            Assert.That(dto.Year, Is.EqualTo(entity.Time.Year));
        }

        [Test]
        public void AssertConfigurations()
        {
            Mapper.CreateMap<ProjectedEntity, ProjectedDto>()
                .ForMember(dest => dest.Day, o => o.MapFrom(src => src.Time.Day))
                .ForMember(dest => dest.Month, o => o.MapFrom(src => src.Time.Month));

            Mapper.AssertConfigurationIsValid();

            var entity = new ProjectedEntity { Time = new DateTime(1900, 05, 05) };
            var dto = Mapper.Map<ProjectedEntity, ProjectedDto>(entity);

            Assert.That(dto.Day, Is.EqualTo(entity.Time.Day));
            Assert.That(dto.Month, Is.EqualTo(entity.Time.Month));
        }

        [Test]
        public void ArraysAndBasicPolymorphism()
        {
            Mapper.CreateMap<SourceValue, DestinationValue>()
                .Include<SourceValueChild, DestinationValueChild>();

            Mapper.CreateMap<SourceValueChild, DestinationValueChild>();
            Mapper.Map<SourceValue, DestinationValue>(new SourceValueChild());
            
            var sources = new IHaveValue[]
	        {
		        new SourceValue { Value = 5 },
		        new SourceValueChild { Value = 6 },
		        new SourceValue { Value = 7 }
	        };

            IEnumerable<DestinationValue> ienumerableDest = Mapper.Map<IHaveValue[], IEnumerable<DestinationValue>>(sources);
            ICollection<DestinationValue> icollectionDest = Mapper.Map<IHaveValue[], ICollection<DestinationValue>>(sources);
            IList<DestinationValue> ilistDest = Mapper.Map<IHaveValue[], IList<DestinationValue>>(sources);
            List<DestinationValue> listDest = Mapper.Map<IHaveValue[], List<DestinationValue>>(sources);
            DestinationValue[] dest = Mapper.Map<IHaveValue[], DestinationValue[]>(sources);

            CollectionAssert.AreEqual(sources, ienumerableDest, new ValueComparer());
            CollectionAssert.AreEqual(sources, icollectionDest, new ValueComparer());
            CollectionAssert.AreEqual(sources, ilistDest, new ValueComparer());
            CollectionAssert.AreEqual(sources, listDest, new ValueComparer());
            CollectionAssert.AreEqual(sources, dest, new ValueComparer());

            Assert.That(listDest[0], Is.InstanceOf<DestinationValue>());
            Assert.That(listDest[1], Is.InstanceOf<DestinationValueChild>());
            Assert.That(listDest[2], Is.InstanceOf<DestinationValue>());
        }

        [Test]
        public void ComplexPolymorphism()
        {
            Mapper.CreateMap<BaseClass, BaseDto>()
                .Include<Class, Dto>()
                .ForMember(dest => dest.IgnoreMe, o => o.Ignore())
                .ForMember(dest => dest.DtoVal1, o => o.MapFrom(src => src.Val1));

            Mapper.CreateMap<Class, Dto>()
                .Include<Subclass, SubDto>()
                .ForMember(dest => dest.DtoVal2, o => o.MapFrom(src => src.Val2));

            Mapper.CreateMap<Subclass, SubDto>()
                .ForMember(dest => dest.DtoVal3, o => o.MapFrom(src => src.Val3));

            Mapper.AssertConfigurationIsValid();

            BaseClass c = new Subclass
                      {
                          Val1 = 1,
                          Val2 = 2,
                          Val3 = 3,
                          IgnoreMe = 100
                      };

            BaseDto result = Mapper.Map<BaseClass, BaseDto>(c);

            Assert.That(result.DtoVal1, Is.EqualTo(1));
            Assert.That((result as Dto).DtoVal2, Is.EqualTo(2));
            Assert.That((result as SubDto).DtoVal3, Is.EqualTo(3));

            // Would fail...
            //Assert.That((result as SubDto).IgnoreMe, Is.EqualTo(0)); 
        }

        [Test]
        public void Customization()
        {
            Mapper.CreateMap<CustomEntity, CustomDto>()
                .ConstructUsing(ConstrcutionMethod)
                //.ConvertUsing(c => new CustomDto())
                //.WithProfile("SomeNonDefaultProfile")
                .BeforeMap(ImBeforeMap)
                .ForMember(dest => dest.ValueAndDescription, o =>
                    o.ResolveUsing(new MyCustomResolver()).FromMember(src => src.Value))
                    //o.NullSubstitute(""))
                    //o.UseDestinationValue())
                    //o.UseValue("Bla Bla Bla"))
                .ForMember(dest => dest.Counter, o => o.Ignore()) // We want to pass the assertion...
                .AfterMap(ImAfterMap);

            Mapper.AssertConfigurationIsValid();

            var entity = new CustomEntity { Value = 1 };
            var dto = Mapper.Map<CustomEntity, CustomDto>(entity);
            Assert.That(dto.ValueAndDescription, Is.EqualTo("the value is 1"));
            Assert.That(dto.Counter, Is.EqualTo(3));
        }

        private void ImAfterMap(CustomEntity arg1, CustomDto arg2)
        {
            arg2.Counter++;
        }

        private void ImBeforeMap(CustomEntity arg1, CustomDto arg2)
        {
            arg2.Counter++;
        }

        private CustomDto ConstrcutionMethod(CustomEntity arg)
        {
            return new CustomDto { Counter = 1 };
        }

        #region Pitfalls

        public void TemporalCoupeling()
        {
            // Harmless, but confusing - try to avoid the static use of Mapper...
            Mapper.CreateMap<BasicEntity, BasicDto>();
            Mapper.Map<BasicEntity, BasicDto>(null);

            // Throws
            Mapper.Map<BasicEntity, BasicDto>(null);
            Mapper.CreateMap<BasicEntity, BasicDto>();

            // Doesn't Throw, yet isn't what we want...
            Mapper.DynamicMap<BasicEntity, BasicDto>(null);
            Mapper.CreateMap<BasicEntity, BasicDto>();
        }

        public void InheritanceOrder()
        {
            // Inheritace works!
            Mapper.CreateMap<Class, Dto>().Include<Subclass, SubDto>();
            Mapper.CreateMap<Subclass, SubDto>();

            // Won't apply include...
            Mapper.CreateMap<Subclass, SubDto>();
            Mapper.CreateMap<Class, Dto>().Include<Subclass, SubDto>();
        }

        public void ContextPerConfigNotPerMap()
        {
            // It may be obvious, but - your session ( \ proxy \ etc) is an instance!
            ISession session = NHibernateSession.Current;
            Mapper.CreateMap<BasicDto, BasicEntity>()
                .ConstructUsing(src => session.Get<BasicEntity>(src.Id));

            // ServiceLocation can help...
            Mapper.CreateMap<BasicDto, BasicEntity>()
                .ConstructUsing(src => NHibernateSession.Current.Get<BasicEntity>(src.Id));
        }

        #endregion
    }

    public class MyCustomResolver : IValueResolver
    {
        public ResolutionResult Resolve(ResolutionResult source)
        {
            return new ResolutionResult(ResolutionContext.New(
                string.Format("the value is {0}", source.Value)));
        }
    }


    public class ProjectedEntity
    {
        public DateTime Time { get; set; }
    }

    public class ProjectedDto
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class BasicEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GetValue() { return 42; }
        public DateTime Time { get; set; }
    }

    public class BasicDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int Value { get; set; }
        public int TimeTicks { get; set; }
    }

    public class SourceValue : IHaveValue
    {
        public int Value { get; set; }
    }

    public class SourceValueChild : SourceValue { }

    public interface IHaveValue
    {
        int Value { get; }
    }

    public class ValueComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return (x as IHaveValue).Value.CompareTo((y as IHaveValue).Value);
        }
    }

    public class DestinationValue : IHaveValue
    {
        public int Value { get; set; }
    }

    public class DestinationValueChild : DestinationValue { }


    public class Subclass : Class
    {
        public int Val3 { get; set; }
    }

    public class Class : BaseClass
    {
        public int Val2 { get; set; }
    }

    public abstract class BaseClass
    {
        public int Val1 { get; set; }
        public int IgnoreMe { get; set; }
    }


    public class SubDto : Dto
    {
        public int DtoVal3 { get; set; }
    }

    public class Dto : BaseDto
    {
        public int DtoVal2 { get; set; }
    }

    public abstract class BaseDto
    {
        public int IgnoreMe { get; set; }
        public int DtoVal1 { get; set; }
    }

    public class CustomEntity
    {
        public int Value { get; set; }
    }

    public class CustomDto
    {
        public string ValueAndDescription { get; set; }

        public int Counter { get; set; }
    }
}