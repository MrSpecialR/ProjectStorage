namespace ProjectStorage.Infrastructure.Configuration
{
    using AutoMapper;
    using System;
    using System.Linq;

    public class SetupAutoMapper : Profile
    {
        public SetupAutoMapper()
        {
            var projectName = this.GetType().Namespace.Split('.')[0];
            var solutionClasses = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.FullName.Contains(projectName))
                ).ToList();

            var items = solutionClasses
                .Where(t => t
                    .GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMappableFrom<>)))
                .Select(t => new
                {
                    Destination = t,
                    Source = t.GetInterfaces().FirstOrDefault(i =>
                           i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMappableFrom<>))
                        .GetGenericArguments().FirstOrDefault()
                })
                .ToList();

            foreach (var cl in items)
            {
                this.CreateMap(cl.Source, cl.Destination);
            }

            solutionClasses.Where(t => t.GetInterfaces().Any(i => i == typeof(ICustomMapConfiguration)))
                .Select(t => new
                {
                    Instance = Activator.CreateInstance(t) as ICustomMapConfiguration
                })
                .ToList().ForEach(c => c.Instance.ConfigureMap(this));
        }
    }
}