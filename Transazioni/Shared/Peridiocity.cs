namespace Transazioni.Domain.Shared;

public enum Peridiocity
{
    None,
    Daily,
    Weekly,
    Monthly,
    Bimonthly,
    Quarterly,
    Semiannual,
    Yearly
}

public static class PeridiocityUtility
{
    public static Peridiocity GetFromString(string angularEnumValue)
    {
        return angularEnumValue switch
        {
            "None" => Peridiocity.None,
            "Daily" => Peridiocity.Daily,
            "Weekly" => Peridiocity.Weekly,
            "Monthly" => Peridiocity.Monthly,
            "Bimonthly" => Peridiocity.Bimonthly,
            "Quarterly" => Peridiocity.Quarterly,
            "Semiannual" => Peridiocity.Semiannual,
            "Yearly" => Peridiocity.Yearly,
            _ => throw new ArgumentException($"Unknown Peridiocity value: {angularEnumValue}")
        };
    }

    public static DateTime GetNextPeridiocityDate(this DateTime currentDate, Peridiocity peridiocity)
    {
        switch (peridiocity)
        {
            case Peridiocity.Daily:
                return currentDate.AddDays(1);
            case Peridiocity.Weekly:
                return currentDate.AddDays(7);
            case Peridiocity.Monthly:
                return currentDate.AddMonths(1);
            case Peridiocity.Bimonthly:
                return currentDate.AddMonths(2);
            case Peridiocity.Quarterly:
                return currentDate.AddMonths(3);
            case Peridiocity.Semiannual:
                return currentDate.AddMonths(6);
            case Peridiocity.Yearly:
                return currentDate.AddYears(1);
            default:
                throw new ArgumentException("Unsupported peridiocity value");
        }
    }
}