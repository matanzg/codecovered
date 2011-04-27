using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

namespace CodeCovered.GeoShop.Infrastructure.AutoMapper
{
    public sealed class ApplyMap
    {
        private readonly IConfiguration _configuration;
        private readonly ICollection<Type> _include;
        private readonly ICollection<Type> _exclude;

        private ApplyMap(IConfiguration configuration)
        {
            _configuration = configuration;
            _include = new List<Type>();
            _exclude = new List<Type>();
        }

        public static ApplyMap On(IConfiguration configuration)
        {
            return new ApplyMap(configuration);
        }

        public ApplyMap Add(Type type)
        {
            if (!_include.Contains(type))
                _include.Add(type);

            return this;
        }

        public ApplyMap Add<T>() where T : ICreateMap
        {
            return Add(typeof(T));
        }

        public ApplyMap Exclude(Type type)
        {
            if (!_exclude.Contains(type))
                _exclude.Add(type);

            return this;
        }

        public ApplyMap Exclude<T>() where T : ICreateMap
        {
            return Exclude(typeof(T));
        }

        public ApplyMap AddFromAssemblyOf<T>()
        {
            return AddFromAssemblyOf(typeof(T));
        }

        private ApplyMap AddFromAssemblyOf(Type type)
        {
            var types = type.Assembly.GetTypes().Where(IsInstancibleClassMap);

            foreach (var mapCreatorType in types)
            {
                _include.Add(mapCreatorType);
            }

            return this;
        }

        private static bool IsInstancibleClassMap(Type type)
        {
            return typeof(ICreateMap).IsAssignableFrom(type) &&
                   type.IsClass && !type.IsAbstract;
        }

        public void Apply()
        {
            var mapCreators = _include.Where(t =>
                        !_exclude.Contains(t) &&
                        !t.IsGenericTypeDefinition &&
                        t.IsClass && !t.IsAbstract &&
                        t.GetConstructor(Type.EmptyTypes) != null);

            var instances = GetHierachicalySortedInstances(mapCreators);

            foreach (var instance in instances)
            {
                instance.CreateMap(_configuration);
            }
        }

        private static IEnumerable<ICreateMap> GetHierachicalySortedInstances(IEnumerable<Type> mapCreators)
        {
            var currentGeneration = mapCreators.Where(TypeHasNoPrecedingType).ToList();
            var remaining = mapCreators.Where(TypeHasPrecedingType).ToList();
            var instances = new List<ICreateMap>(mapCreators.Count());

            do
            {
                instances.AddRange(currentGeneration.Select(Activator.CreateInstance).OfType<ICreateMap>());

                List<Type> generation = currentGeneration;
                var nextGeneration = remaining.FindAll(t => IsPrecedingContained(generation, t));

                remaining.RemoveAll(nextGeneration.Contains);
                currentGeneration = nextGeneration;
            } while (currentGeneration.Count > 0);

            if (remaining.Any())
                throw new AutoMapperMappingException(CreateExceptionMessageFor(remaining[0]));

            return instances;
        }

        private static bool IsPrecedingContained(IEnumerable<Type> generation, Type type)
        {
            var dependency = type.GetInterface(typeof(ICreateMapAfter<>).Name, true).GetGenericArguments()[0];
            return generation.Contains(dependency);
        }

        private static string CreateExceptionMessageFor(Type type)
        {
            var dependency = type.GetInterface(typeof(ICreateMapAfter<>).Name, true).GetGenericArguments()[0];
            return string.Format("The type {0} was added, but it's preceding type {1} was not", type.Name, dependency.Name);
        }

        private static bool TypeHasNoPrecedingType(Type type)
        {
            return !TypeHasPrecedingType(type);
        }

        private static bool TypeHasPrecedingType(Type type)
        {
            return type.GetInterface(typeof (ICreateMapAfter<>).Name) != null;
        }
    }
}