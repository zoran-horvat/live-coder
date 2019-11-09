using System;
using System.ComponentModel.Design;
using LiveCoder.Common.Optional;

namespace LiveCoder.Extension
{
    static class ServiceProviderExtensions
    {
        public static Option<TService> TryGetService<TService>(this IServiceProvider provider) =>
            provider.GetService(typeof(IMenuCommandService))
                .FromNullable()
                .OfType<TService>();
    }
}
