using Castle.DynamicProxy;
using DryIoc;
using DryIoc.ImTools;
using IoC.Interceptors.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.MS.Interceptors.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly DefaultProxyBuilder ProxyBuilder = new();
    private static readonly ProxyGenerator ProxyGenerator = new();

    public static void Intercept(this IServiceCollection builder, Type serviceType, Type interceptorType)
    {
        var closedServiceTypes = serviceType.IsClosedGeneric()
            ? new[] { serviceType }
            : new List<ServiceDescriptor>(builder)
                .Select(candidateRegistration => candidateRegistration.ServiceType)
                .Where(candidateType => candidateType.IsClosedTypeOf(serviceType))
                .SelectMany(candidateType => candidateType.GetTypesThatClose(serviceType))
                .Distinct();


        foreach (var closedServiceType in closedServiceTypes)
        {
            var concrete = builder.BuildServiceProvider()
                .GetRequiredService(closedServiceType);

            Func<IServiceProvider, object> factory = (provider) =>
            {
                Type implType;
                var interceptor = provider.GetRequiredService(interceptorType);

                if (closedServiceType.IsInterface)
                    implType = ProxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(
                        closedServiceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
                else if (closedServiceType.IsClass)
                    implType = ProxyBuilder.CreateClassProxyTypeWithTarget(
                        closedServiceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
                else
                    throw new ArgumentException(
                        $"Intercepted service type {serviceType} is not a supported, cause it is nor a class nor an interface");
                var ctor = implType.PublicConstructors().Where(ctor => ctor.GetParameters().Length != 0).First();

                return ctor.Invoke(new[] { new[] { interceptor as IInterceptor }, concrete });
            };

            var serviceDescriptor = new ServiceDescriptor(closedServiceType, factory, ServiceLifetime.Transient);
            builder.Add(serviceDescriptor);
        }
    }
}