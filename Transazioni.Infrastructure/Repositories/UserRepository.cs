using Microsoft.EntityFrameworkCore;
using Transazioni.Domain.Users;

namespace Transazioni.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext DbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Add(User user)
    {
        DbContext.Set<User>().Add(user);
    }

    public async Task<User?> GetByAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
            .FirstOrDefaultAsync(b => b.Email == email, cancellationToken);
    }
}