namespace Transazioni.Domain.Utilities.Pagination;

public static class Paginator
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> list, PaginationConfigurations? configurations = null)
    {
        if (list == null)
            throw new ArgumentNullException(nameof(list));

        configurations ??= new PaginationConfigurations();

        int pageNumber = configurations.pageNumber < 1 ? 1 : configurations.pageNumber;
        int pageSize = configurations.pageSize < 1 ? 10 : configurations.pageSize;

        int skip = (pageNumber - 1) * pageSize;
        return list.Skip(skip).Take(pageSize);
    }
}
