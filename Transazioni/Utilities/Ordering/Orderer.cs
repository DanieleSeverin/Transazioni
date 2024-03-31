using System.Reflection;
using Transazioni.Domain.Account;

namespace Transazioni.Domain.Utilities.Ordering;

public static class Orderer
{
    public static IEnumerable<T> OrderByProperty<T>(this IEnumerable<T> source, OrderingConfigurations? configurations = null)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (source.Count() < 2)
            return source;

        configurations ??= new OrderingConfigurations();

        if (string.IsNullOrWhiteSpace(configurations.propertyName))
            return source;

        PropertyInfo? propertyInfo = typeof(T).GetProperty(configurations.propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null)
            throw new ArgumentException($"Property '{configurations.propertyName}' not found on type '{typeof(T).Name}'.", nameof(configurations.propertyName));

        if (!propertyInfo.CanRead)
            throw new InvalidOperationException($"Property '{configurations.propertyName}' does not have a getter.");

        bool implementsIComparable = typeof(IComparable).IsAssignableFrom(propertyInfo.PropertyType);
        if (!implementsIComparable)
        {
            throw new InvalidOperationException($"{propertyInfo.Name} does not implement IComparable.");
        }

        Func<T, object?> keySelector = item => propertyInfo.GetValue(item);

        return configurations.ascending ? 
            source.OrderBy(keySelector) : 
            source.OrderByDescending(keySelector);
    }
}
