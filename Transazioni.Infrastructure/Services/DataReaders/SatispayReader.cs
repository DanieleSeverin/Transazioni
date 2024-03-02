using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Transazioni.Application.Satispay.UploadSatispayMovements;
using Transazioni.Domain.Extensions;
using Transazioni.Domain.Satispay;

namespace Transazioni.Infrastructure.Services.DataReaders;

public class SatispayReader : ISatispayReader
{
    public List<SatispayMovements> ReadMovements(IFormFile File)
    {
        List<SatispayMovements> response = new List<SatispayMovements>();

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
                    .Equals("ID", StringComparison.OrdinalIgnoreCase))
                {
                    startRow++;
                }

                // Iterate through each row starting from the headers
                for (int row = startRow + 1; row <= worksheet.LastRowUsed().RowNumber(); row++)
                {
                    // Access data in each column for the current row
                    IXLCell nameCell = worksheet.Cell(row, startColumn + 1);
                    IXLCell dateCell = worksheet.Cell(row, startColumn + 4);
                    IXLCell amountCell = worksheet.Cell(row, startColumn + 5);
                    IXLCell currencyCell = worksheet.Cell(row, startColumn + 6);

                    ClosedXmlParseOptions dateOptions = new()
                    {
                        DateFormat = "dd MMM yyyy",
                        CultureInfo = new CultureInfo("it-IT")
                    };

                    dateCell.Value = dateCell.Value.ToString().Split(",")[0];

                    response.Add(new SatispayMovements()
                    {
                        Date = dateCell.ToDateTime(dateOptions),
                        Name = nameCell.Value.ToString(),
                        Currency = currencyCell.Value.ToString(),
                        Amount = amountCell.ToDecimal() ?? 0
                    });
                }
            }
        }

        return response;
    }
}
