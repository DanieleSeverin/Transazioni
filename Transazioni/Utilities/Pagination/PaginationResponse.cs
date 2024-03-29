namespace Transazioni.Domain.Utilities.Pagination;

public record PaginationResponse<T>(int count, IEnumerable<T> list);