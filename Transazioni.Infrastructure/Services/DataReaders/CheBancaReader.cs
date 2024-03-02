using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Transazioni.Application.CheBanca.UploadCheBancaMovements;
using Transazioni.Domain.CheBanca;
using Transazioni.Domain.Extensions;

namespace Transazioni.Infrastructure.Services.DataReaders;

public class CheBancaReader : ICheBancaReader
{
    public List<CheBancaMovements> ReadMovements(IFormFile File)
    {
        List<CheBancaMovements> response = new List<CheBancaMovements>();

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
                int startColumn = 2;
                while (!worksheet.Cell(startRow, startColumn).Value.ToString()
                    .Equals("Data contabile", StringComparison.OrdinalIgnoreCase))
                {
                    startRow++;
                }

                // Iterate through each row starting from the headers
                for (int row = startRow + 1; row <= worksheet.LastRowUsed().RowNumber() - 3; row++)
                {
                    // Access data in each column for the current row
                    IXLCell dataContabileCell = worksheet.Cell(row, startColumn);
                    IXLCell dataValutaCell = worksheet.Cell(row, startColumn + 1);
                    IXLCell tipologiaCell = worksheet.Cell(row, startColumn + 2);
                    IXLCell entrateCell = worksheet.Cell(row, startColumn + 3);
                    IXLCell usciteCell = worksheet.Cell(row, startColumn + 4);
                    IXLCell divisaCell = worksheet.Cell(row, startColumn + 5);

                    response.Add(new CheBancaMovements()
                    {
                        DataContabile = dataContabileCell.ToDateTime(),
                        DataValuta = dataValutaCell.ToDateTime(),
                        Tipologia = tipologiaCell.Value.ToString(),
                        Entrate = entrateCell.ToDecimal(),
                        Uscite = usciteCell.ToDecimal(),
                        Divisa = divisaCell.Value.ToString()
                    });
                }
            }

        }

        return response;
    }
}
