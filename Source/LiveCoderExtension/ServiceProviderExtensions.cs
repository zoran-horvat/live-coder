using System;
using System.ComponentModel.Design;
using Common.Optional;

namespace LiveCoderExtension
{
    static class ServiceProviderExtensions
    {
        public static Option<TService> TryGetService<TService>(this IServiceProvider provider) =>
            provider.GetService(typeof(IMenuCommandService))
                .FromNullable()
                .OfType<TService>();
    }
}
