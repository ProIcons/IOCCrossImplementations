using Castle.DynamicProxy;
using DryIoc;
using DryIoc.ImTools;
using IoC.Interceptors.Common.Extensions;

namespace IoC.Interceptors.DryIoc.Extensions;

public static class DryIocInterceptionExtensions
{
    private static readonly DefaultProxyBuilder ProxyBuilder = new();

    public static void Intercept(this IRegistrator builder, Type serviceType, Type interceptorType)
    {
        var closedServiceTypes = serviceType.IsClosedGeneric()
            ? new[] { serviceType }
            : builder.GetServiceRegistrations()
                .Select(candidateRegistration => candidateRegistration.ServiceType)
                .Where(candidateType => candidateType.IsClosedTypeOf(serviceType))
                .SelectMany(candidateType => candidateType.GetTypesThatClose(serviceType))
                .Distinct();

        foreach (var closedServiceType in closedServiceTypes)
        {
            Type implType;
            if (closedServiceType.IsInterface)
                implType = ProxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(
                    closedServiceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            else if (closedServiceType.IsClass)
                implType = ProxyBuilder.CreateClassProxyTypeWithTarget(
                    closedServiceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            else
                throw new ArgumentException(
                    $"Intercepted service type {serviceType} is not a supported, cause it is nor a class nor an interface");

            builder.Register(closedServiceType, implType,
                made: Made.Of(pt => pt.PublicConstructors().FindFirst(ctor => ctor.GetParameters().Length != 0),
                    Parameters.Of.Type<IInterceptor[]>(interceptorType.MakeArrayType())),
                setup: Setup.DecoratorOf(useDecorateeReuse: true));
        }
    }
}