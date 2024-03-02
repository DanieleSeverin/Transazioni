namespace Transazioni.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByAsync(UserId id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    void Add(User user);
}