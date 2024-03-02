using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transazioni.Domain.Account;

namespace Transazioni.Infrastructure.Configurations;

internal sealed class AccountConfigurations : IEntityTypeConfiguration<Accounts>
{
    public void Configure(EntityTypeBuilder<Accounts> builder)
    {
        builder.HasKey(account => account.Id);

        builder.Property(account => account.Id)
            .HasConversion(accountId => accountId.Value, value => new AccountId(value));

        builder.Property(account => account.AccountName)
            .HasMaxLength(200)
            .HasConversion(accountName => accountName.Value, value => new AccountName(value));

        builder.HasMany(account => account.Movements)
            .WithOne()
            .HasForeignKey(movement => movement.AccountId)
            .HasPrincipalKey(account => account.Id);

        builder.HasMany(account => account.DestinationMovements)
            .WithOne()
            .HasForeignKey(movement => movement.DestinationAccountId)
            .HasPrincipalKey(account => account.Id);
    }
}
