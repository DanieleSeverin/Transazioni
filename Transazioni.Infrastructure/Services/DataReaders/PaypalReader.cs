using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Transazioni.Application.Paypal.UploadPaypalMovements;
using Transazioni.Domain.Extensions;
using Transazioni.Domain.Paypal;

namespace Transazioni.Infrastructure.Services.DataReaders;

public class PaypalReader : IPaypalReader
{
    public List<PaypalMovements> ReadMovements(IFormFile File)
    {
        List<PaypalMovements> response = new List<PaypalMovements>();

        using (var stream = new MemoryStream())
        {
            // Copy the content of the IFormFile into a MemoryStream
            File.CopyTo(stream);
            stream.Position = 0; // Reset the stream position

            using (var package = new XLWorkbook(stream))
            {
                // Assuming you're reading from the first worksheet in the Excel file
                var worksheet = package.Worksheet(1);

                // Find the row index where the headers are located
                int startRow = 1;
                int startColumn = 1;
                while (!worksheet.Cell(startRow, startColumn).Value.ToString()
                    .Contains("Data", StringComparison.OrdinalIgnoreCase))
                {
                    startRow++;
                }

                // Iterate through each row starting from the headers
                for (int row = startRow + 1; row <= worksheet.LastRowUsed().RowNumber(); row++)
                {
                    // Access data in each column for the current row
                    IXLCell dataCell = worksheet.Cell(row, startColumn);
                    IXLCell descrizioneCell = worksheet.Cell(row, startColumn + 3);
                    IXLCell valutaCell = worksheet.Cell(row, startColumn + 4);
                    IXLCell lordoCell = worksheet.Cell(row, startColumn + 5);
                    IXLCell nomeCell = worksheet.Cell(row, startColumn + 11);
                    IXLCell nomeBancaCell = worksheet.Cell(row, startColumn + 12);

                    ClosedXmlParseOptions dateOptions = new()
                    {
                        DateFormat = GetDateFormat(dataCell.Value.ToString().Replace(" 00:00:00", ""))
                    };

                    response.Add(new PaypalMovements()
                    {
                        Data = dataCell.ToDateTime(dateOptions),
                        Descrizione = descrizioneCell.Value.ToString(),
                        Valuta = valutaCell.Value.ToString(),
                        Nome = nomeCell.Value.ToString(),
                        NomeBanca = nomeBancaCell.Value.ToString(),
                        Lordo = lordoCell.ToDecimal() ?? 0
                    });
                }
            }
        }

        return response;
    }

    private string GetDateFormat(string dateString)
    {
        if(dateString.Length == 9)
        {
            return "dd/M/yyyy";
        }

        if (dateString.Length == 10)
        {
            return "dd/MM/yyyy";
        }

        throw new ArgumentOutOfRangeException($"Parameter {nameof(dateString)} lenght should be 9 or 10." +
            $"Input: '{dateString}'.");
    }
}
