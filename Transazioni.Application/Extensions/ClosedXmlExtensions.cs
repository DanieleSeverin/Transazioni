using ClosedXML.Excel;
using Serilog;
using System.Globalization;

namespace Transazioni.Domain.Extensions;

public class ClosedXmlParseOptions
{
    public string DateFormat { get; init; } = "dd/MM/yyyy";
    public CultureInfo CultureInfo { get; init; } = CultureInfo.InvariantCulture;
    public DateTimeStyles DateTimeStyle { get; init; } = DateTimeStyles.None;
}

public static class ClosedXmlExtensions
{
    private static readonly ILogger _logger = Log.ForContext<ClosedXmlParseOptions>();

    public static DateTime ToDateTime(this IXLCell DateString, ClosedXmlParseOptions? Options = null)
    {
        Options ??= new ClosedXmlParseOptions();

        var dateString = DateString.Value.ToString().Replace(" 00:00:00", "");

        try
        {
            return DateTime.ParseExact(
                        dateString,
                        Options.DateFormat,
                        Options.CultureInfo,
                        Options.DateTimeStyle);
        }
        catch (FormatException ex)
        {
            _logger.Error(ex, "FormatException trying to parse {Input} to DateTime", dateString);
            throw;
        }
    }

    public static decimal? ToDecimal(this IXLCell DoubleString)
    {
        string stringNumber = DoubleString.Value.ToString().Replace(",", ".");

        if(string.IsNullOrWhiteSpace(stringNumber))
        {
            return null;
        }


        try
        {
            return decimal.Parse(
                stringNumber,
                NumberStyles.Float,
                NumberFormatInfo.InvariantInfo
                );
        } catch (FormatException ex)
        {
            _logger.Error(ex, "FormatException trying to parse {Input} to decimal", stringNumber);
            throw;
        }

    }
}
