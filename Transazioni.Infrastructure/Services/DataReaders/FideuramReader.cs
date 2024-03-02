using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Transazioni.Application.Fideuram.UploadFideuramMovements;
using Transazioni.Domain.Extensions;
using Transazioni.Domain.Fideuram;

namespace Transazioni.Infrastructure.Services.DataReaders;

public class FideuramReader : IFideuramReader
{
    public List<FideuramMovements> ReadMovements(IFormFile File)
    {
        List<FideuramMovements> response = new List<FideuramMovements>();

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
                    .Equals("Data", StringComparison.OrdinalIgnoreCase))
                {
                    startRow++;
                }

                // Iterate through each row starting from the headers
                for (int row = startRow + 1; row <= worksheet.LastRowUsed().RowNumber(); row++)
                {
                    // Access data in each column for the current row
                    IXLCell dataCell = worksheet.Cell(row, startColumn);
                    IXLCell operazioneCell = worksheet.Cell(row, startColumn + 1);
                    IXLCell dettagliCell = worksheet.Cell(row, startColumn + 2);
                    IXLCell categoriaCell = worksheet.Cell(row, startColumn + 5);
                    IXLCell valutaCell = worksheet.Cell(row, startColumn + 6);
                    IXLCell importoCell = worksheet.Cell(row, startColumn + 7);

                    response.Add(new FideuramMovements()
                    {
                        Data = dataCell.ToDateTime(),
                        Operazione = operazioneCell.Value.ToString(),
                        Dettagli = dettagliCell.Value.ToString(),
                        Categoria = categoriaCell.Value.ToString(),
                        Valuta = valutaCell.Value.ToString(),
                        Importo = importoCell.ToDecimal() ?? 0
                    });
                }
            }
        }

        return response;
    }
}
