using Microsoft.EntityFrameworkCore;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;
using Transazioni.Domain.Users;
using Transazioni.Infrastructure;

namespace Transazioni.API.Extensions;

public static class SeedDataExtensions
{
    public async static Task SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var firstUser = await DbContext.Set<User>().FirstOrDefaultAsync();

        if (firstUser is null || await DbContext.Set<AccountRules>().AnyAsync())
        {
            return;
        }

        DbContext.AddRange(new List<AccountRules>()
        {
            new AccountRules(
                new RuleContains("Satispay"), 
                new AccountName("Satispay"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("PayPal"),
                new AccountName("PayPal"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("Glovo"),
                new AccountName("Glovo"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("JustEat"),
                new AccountName("JustEat"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("AMAZON"),
                new AccountName("Amazon"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("AMZN"),
                new AccountName("Amazon"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("Vinted"),
                new AccountName("Vinted"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("Addebito canone"),
                new AccountName("Canone"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("PRELIEVO"),
                new AccountName("Prelievi"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("Stipendio"),
                new AccountName("Stipendio"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("Disposizione - RIF:"),
                new AccountName("Disposizioni"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("Bonif"),
                new AccountName("Bonifici"),
                firstUser.Id),
            
            new AccountRules(
                new RuleContains("PAGAMENTO POS"),
                new AccountName("Pagamenti POS"),
                firstUser.Id)
        });

        await DbContext.SaveChangesAsync();
    }
}
