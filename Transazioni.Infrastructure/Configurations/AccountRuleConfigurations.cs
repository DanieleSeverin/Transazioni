using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;

namespace Transazioni.Infrastructure.Configurations;

internal sealed class AccountRuleConfigurations : IEntityTypeConfiguration<AccountRules>
{
    public void Configure(EntityTypeBuilder<AccountRules> builder)
    {
        builder.HasKey(rule => rule.Id);

        builder.Property(rule => rule.Id)
            .HasConversion(ruleId => ruleId.Value, value => new AccountRuleId(value));

        builder.Property(rule => rule.RuleContains)
            .HasMaxLength(200)
            .HasConversion(ruleContains => ruleContains.Value, value => new RuleContains(value));

        builder.Property(rule => rule.AccountName)
            .HasMaxLength(200)
            .HasConversion(accountName => accountName.Value, value => new AccountName(value));
    }
}
