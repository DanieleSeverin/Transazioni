using System.Reflection;

namespace Transazioni.Domain.Utilities.Ordering;

public static class Orderer
{
    public static IEnumerable<T> OrderByProperty<T>(this IEnumerable<T> source, OrderingConfigurations? configurations = null)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        configurations ??= new OrderingConfigurations();

        if (string.IsNullOrWhiteSpace(configurations.propertyName))
            return source;

        PropertyInfo? propertyInfo = typeof(T).GetProperty(configurations.propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null)
            throw new ArgumentException($"Property '{configurations.propertyName}' not found on type '{typeof(T).Name}'.", nameof(configurations.propertyName));

        if (!propertyInfo.CanRead)
            throw new InvalidOperationException($"Property '{configurations.propertyName}' does not have a getter.");

        Func<T, object?> keySelector = item => propertyInfo.GetValue(item);

        return configurations.ascending ? 
            source.OrderBy(keySelector) : 
            source.OrderByDescending(keySelector);
    }
}
