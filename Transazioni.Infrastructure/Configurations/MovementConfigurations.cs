using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;

namespace Transazioni.Infrastructure.Configurations;

internal sealed class MovementConfigurations : IEntityTypeConfiguration<Movements>
{
    public void Configure(EntityTypeBuilder<Movements> builder)
    {
        builder.HasKey(movement => movement.Id);

        builder.Property(movement => movement.Id)
            .HasConversion(movementId => movementId.Value, value => new MovementId(value));

        builder.Property(movement => movement.Description)
            .HasMaxLength(400)
            .HasConversion(description => description.Value, value => new MovementDescription(value));

        builder.OwnsOne(movement => movement.Money, moneyBuilder =>
        {
            moneyBuilder.Property(money => money.Amount)
                .HasColumnName("Amount");

            moneyBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code))
                .HasColumnName("Currency");
        });

        builder.Property(movement => movement.Category)
            .HasMaxLength(100)
            .HasConversion(category => category != null ? category.Value : null,
                           value => value != null ? new MovementCategory(value) : null);

        builder.Property(movement => movement.AccountId).HasConversion(
            accountId => accountId.Value,
            value => new AccountId(value));

        builder.HasOne(movement => movement.Account)
            .WithMany(account => account.Movements)
            .HasForeignKey(movement => movement.AccountId)
            .HasPrincipalKey(account => account.Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(movement => movement.DestinationAccountId).HasConversion(
            destinationAccountId => destinationAccountId.Value,
            value => new AccountId(value));

        builder.HasOne(movement => movement.DestinationAccount)
            .WithMany(account => account.DestinationMovements)
            .HasForeignKey(movement => movement.DestinationAccountId)
            .HasPrincipalKey(account => account.Id)
            .OnDelete(DeleteBehavior.SetNull);
    }
}