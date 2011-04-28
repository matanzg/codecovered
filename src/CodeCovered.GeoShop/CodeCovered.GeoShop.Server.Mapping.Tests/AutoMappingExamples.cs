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
            Mapper.CreateMap<ArraySource, ArrayDestination>()
                .Include<ArraySourceChild, ArrayDestinationChild>();

            Mapper.CreateMap<ArraySourceChild, ArrayDestinationChild>();

            var sources = new[]
	        {
		        new ArraySource { Value = 5 },
		        new ArraySourceChild { Value = 6 },
		        new ArraySource { Value = 7 }
	        };

            IEnumerable<ArrayDestination> ienumerableDest = Mapper.Map<ArraySource[], IEnumerable<ArrayDestination>>(sources);
            ICollection<ArrayDestination> icollectionDest = Mapper.Map<ArraySource[], ICollection<ArrayDestination>>(sources);
            IList<ArrayDestination> ilistDest = Mapper.Map<ArraySource[], IList<ArrayDestination>>(sources);
            List<ArrayDestination> listDest = Mapper.Map<ArraySource[], List<ArrayDestination>>(sources);
            ArrayDestination[] arrayDest = Mapper.Map<ArraySource[], ArrayDestination[]>(sources);

            CollectionAssert.AreEqual(sources, ienumerableDest, new ValueComparer());
            CollectionAssert.AreEqual(sources, icollectionDest, new ValueComparer());
            CollectionAssert.AreEqual(sources, ilistDest, new ValueComparer());
            CollectionAssert.AreEqual(sources, listDest, new ValueComparer());
            CollectionAssert.AreEqual(sources, arrayDest, new ValueComparer());

            Assert.That(listDest[0], Is.InstanceOf<ArrayDestination>());
            Assert.That(listDest[1], Is.InstanceOf<ArrayDestinationChild>());
            Assert.That(listDest[2], Is.InstanceOf<ArrayDestination>());
        }

        [Test]
        public void ComplexPolymorphism()
        {
            Mapper.CreateMap<A, DtoA>()
                .Include<B, DtoB>()
                .ForMember(dest => dest.IgnoreMe, o => o.Ignore())
                .ForMember(dest => dest.DtoVal1, o => o.MapFrom(src => src.Val1));

            Mapper.CreateMap<B, DtoB>()
                .Include<C, DtoC>()
                .ForMember(dest => dest.DtoVal2, o => o.MapFrom(src => src.Val2));

            Mapper.CreateMap<C, DtoC>()
                .ForMember(dest => dest.DtoVal3, o => o.MapFrom(src => src.Val3));

            Mapper.AssertConfigurationIsValid();

            A c = new C
                      {
                          Val1 = 1,
                          Val2 = 2,
                          Val3 = 3,
                          IgnoreMe = 100
                      };

            DtoA result = Mapper.Map<A, DtoA>(c);

            Assert.That(result.DtoVal1, Is.EqualTo(1));
            Assert.That((result as DtoB).DtoVal2, Is.EqualTo(2));
            Assert.That((result as DtoC).DtoVal3, Is.EqualTo(3));

            // Would probably fail...
            //Assert.That((result as DtoC).IgnoreMe, Is.EqualTo(0)); 
        }

        [Test]
        public void Customization()
        {
            Mapper.CreateMap<CustomEntity, CustomDto>()
                .ConstructUsing(ConstructUsingMe)
                .BeforeMap(ImBeforeMap)
                .ForMember(dest => dest.ValueAndDescription, o =>
                    o.ResolveUsing(new MyCustomResolver()).FromMember(src => src.Value))
                .ForMember(dest => dest.Counter, o => o.Ignore())
                .AfterMap(ImAfterMap);

            Mapper.AssertConfigurationIsValid();

            var entity = new CustomEntity { Value = 1 };
            var dto = Mapper.Map<CustomEntity, CustomDto>(entity);
            Assert.That(dto.ValueAndDescription, Is.EqualTo("the value is 1"));
            Assert.That(dto.Counter, Is.EqualTo(3));
        }

        #region boring crap

        private void ImAfterMap(CustomEntity arg1, CustomDto arg2)
        {
            arg2.Counter++;
        }

        private void ImBeforeMap(CustomEntity arg1, CustomDto arg2)
        {
            arg2.Counter++;
        }

        private CustomDto ConstructUsingMe(CustomEntity arg)
        {
            return new CustomDto { Counter = 1 };
        }

        #endregion

        #region Pitfalls

        public void Pitfalls()
        {
            // Harmless, but confusing - try to avoid the static use of Mapper...
            Mapper.CreateMap<BasicEntity, BasicDto>();
            Mapper.Map<BasicEntity, BasicDto>(null);

            // Won't apply include
            Mapper.CreateMap<C, DtoC>();
            Mapper.CreateMap<B, DtoB>().Include<C, DtoC>();

            // It may be obvious, but - your session ( \ proxy \ etc) is an instance!
            ISession session = NHibernateSession.Current;
            Mapper.CreateMap<BasicDto, BasicEntity>()
                .ConstructUsing(src => session.Get<BasicEntity>(src.Id));

            // a better use:
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

    public class ArraySource : IHaveValue
    {
        public int Value { get; set; }
    }

    public class ArraySourceChild : ArraySource { }

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

    public class ArrayDestination : IHaveValue
    {
        public int Value { get; set; }
    }

    public class ArrayDestinationChild : ArrayDestination { }


    public class C : B
    {
        public int Val3 { get; set; }
    }

    public class B : A
    {
        public int Val2 { get; set; }
    }

    public class A
    {
        public int Val1 { get; set; }
        public int IgnoreMe { get; set; }
    }


    public class DtoC : DtoB
    {
        public int DtoVal3 { get; set; }
    }

    public class DtoB : DtoA
    {
        public int DtoVal2 { get; set; }
    }

    public class DtoA
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