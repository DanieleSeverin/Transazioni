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

        var users = await DbContext.Set<User>().ToListAsync();

        if (!users.Any())
        {
            return;
        }

        DbContext.AddRange(new List<AccountRules>()
        {
            new AccountRules(
                new RuleContains("Satispay"), 
                new AccountName("Satispay"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("PayPal"),
                new AccountName("PayPal"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("Glovo"),
                new AccountName("Glovo"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("JustEat"),
                new AccountName("JustEat"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("AMAZON"),
                new AccountName("Amazon"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("AMZN"),
                new AccountName("Amazon"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("Vinted"),
                new AccountName("Vinted"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("Addebito canone"),
                new AccountName("Canone"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("PRELIEVO"),
                new AccountName("Prelievi"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("Stipendio"),
                new AccountName("Stipendio"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("Disposizione - RIF:"),
                new AccountName("Disposizioni"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("Bonif"),
                new AccountName("Bonifici"),
                users[0].Id),
            
            new AccountRules(
                new RuleContains("PAGAMENTO POS"),
                new AccountName("Pagamenti POS"),
                users[0].Id)
        });

        await DbContext.SaveChangesAsync();
    }
}
