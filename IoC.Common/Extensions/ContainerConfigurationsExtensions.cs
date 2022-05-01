using Autofac;
using Autofac.Extras.DynamicProxy;
using DryIoc;
using IoC.Common.Handlers;
using IoC.Common.Requests;
using IoC.Common.Responses;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Common.Extensions;

public static class ContainerConfigurationsExtensions
{
    public static IServiceCollection AddCommonToMicrosoftDependencyInjection(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddMediatR(typeof(Common).Assembly);

    public static ContainerBuilder AddCommonsToAutofac(this ContainerBuilder builder, bool initializeMediatR = true)
    {
        if (initializeMediatR)
        {
            builder
                .RegisterMediatR(typeof(Common).Assembly);
        }
        return builder;
    }

    public static Container AddCommonsToDryIoc(this Container container)
    {
        container.RegisterDelegate<ServiceFactory>(r => r.Resolve);
        container.RegisterMany(new[] { typeof(IMediator).GetAssembly() }, Registrator.Interfaces);
        container.RegisterMany(typeof(Common).GetAssembly().GetTypes().Where(t => t.IsMediatorHandler()));

        container.Register(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>),
            ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
        container.Register(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>),
            ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);

        return container;
    }

    public static bool IsMediatorHandler(this Type arg)
    {
        return arg
            .GetInterfaces()
            .Any(i => i.Name.StartsWith("IRequestHandler"));
    }
}