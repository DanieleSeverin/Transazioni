using ClosedXML.Excel;
using System.Globalization;

namespace Transazioni.Domain.Extensions;

public class ClosedXmlParseOptions
{
    public string DateFormat { get; init; } = "dd/MM/yyyy";
    public CultureInfo CultureInfo { get; init; } = CultureInfo.InvariantCulture;
    public DateTimeStyles DateTimeStyle { get; init; } = DateTimeStyles.None;

    public NumberFormatInfo NumberFormatInfo { get; init; } = new NumberFormatInfo
    {
        NumberDecimalSeparator = ","
    };
}

public static class ClosedXmlExtensions
{
    public static DateTime ToDateTime(this IXLCell DateString, ClosedXmlParseOptions? Options = null)
    {
        Options ??= new ClosedXmlParseOptions();

        var dateString = DateString.Value.ToString().Replace(" 00:00:00", "");

        return DateTime.ParseExact(
                    dateString,
                    Options.DateFormat,
                    Options.CultureInfo,
                    Options.DateTimeStyle);
    }

    public static decimal? ToDecimal(this IXLCell DoubleString, ClosedXmlParseOptions? Options = null)
    {
        Options ??= new ClosedXmlParseOptions();

        string stringNumber = DoubleString.Value.ToString();

        if(string.IsNullOrWhiteSpace(stringNumber))
        {
            return null;
        }

        return decimal.Parse(
            stringNumber,
            NumberStyles.Float,
            Options.NumberFormatInfo);
    }
}
