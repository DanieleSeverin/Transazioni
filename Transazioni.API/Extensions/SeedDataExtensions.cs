using Microsoft.EntityFrameworkCore;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;
using Transazioni.Infrastructure;

namespace Transazioni.API.Extensions;

public static class SeedDataExtensions
{
    public async static Task SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var rules = await DbContext.Set<AccountRules>().ToListAsync();

        if (rules.Any())
        {
            return;
        }

        DbContext.AddRange(new List<AccountRules>()
        {
            new AccountRules(
                new RuleContains("Satispay"), 
                new AccountName("Satispay")),
            new AccountRules(
                new RuleContains("PayPal"),
                new AccountName("PayPal")),
            new AccountRules(
                new RuleContains("Glovo"),
                new AccountName("Glovo")),
            new AccountRules(
                new RuleContains("JustEat"),
                new AccountName("JustEat")),
            new AccountRules(
                new RuleContains("AMAZON"),
                new AccountName("Amazon")),
            new AccountRules(
                new RuleContains("AMZN"),
                new AccountName("Amazon")),
            new AccountRules(
                new RuleContains("Vinted"),
                new AccountName("Vinted")),
            new AccountRules(
                new RuleContains("Addebito canone"),
                new AccountName("Canone")),
            new AccountRules(
                new RuleContains("PRELIEVO"),
                new AccountName("Prelievi")),
            new AccountRules(
                new RuleContains("Stipendio"),
                new AccountName("Stipendio")),
            new AccountRules(
                new RuleContains("Disposizione - RIF:"),
                new AccountName("Disposizioni")),
            new AccountRules(
                new RuleContains("Bonif"),
                new AccountName("Bonifici")),
            new AccountRules(
                new RuleContains("PAGAMENTO POS"),
                new AccountName("Pagamenti POS"))
        });

        await DbContext.SaveChangesAsync();
    }
}
