// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace IoC.Interceptors.Common.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<Type> GetTypesThatClose(this Type @this, Type openGeneric)
    {
        return FindAssignableTypesThatClose(@this, openGeneric);
    }

    public static bool IsClosedTypeOf(this Type @this, Type openGeneric)
    {
        return TypesAssignableFrom(@this).Any(t => t.IsGenericType && !@this.ContainsGenericParameters && t.GetGenericTypeDefinition() == openGeneric);
    }

    private static IEnumerable<Type> FindAssignableTypesThatClose(Type candidateType, Type openGenericServiceType)
    {
        return TypesAssignableFrom(candidateType)
            .Where(t => t.IsClosedTypeOf(openGenericServiceType));
    }

    private static IEnumerable<Type> TypesAssignableFrom(Type candidateType)
    {
        return candidateType.GetInterfaces().Concat(
            Across(candidateType, t => t.BaseType!));
    }

    private static IEnumerable<T> Across<T>(T first, Func<T, T> next)
        where T : class
    {
        var item = first;
        while (item != null)
        {
            yield return item;
            item = next(item);
        }
    }
}