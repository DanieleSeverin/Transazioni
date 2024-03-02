using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Globalization;
using Transazioni.Application.Santander.uploadSantanderMovements;
using Transazioni.Domain.Santander;

namespace Transazioni.Infrastructure.Services.DataReaders;

public class SantanderReader : ISantanderReader
{
    //public List<SantanderMovements> ReadMovements(IFormFile File)
    //{
    //    List<SantanderMovements> response = new List<SantanderMovements>();

    //    using (var stream = new MemoryStream())
    //    {
    //        // Copy the content of the IFormFile into a MemoryStream
    //        File.CopyTo(stream);
    //        stream.Position = 0; // Reset the stream position

    //        using (var package = new XLWorkbook(stream))
    //        {
    //            // Assuming you're reading from the first worksheet in the Excel file
    //            var worksheet = package.Worksheet(1);

    //            // Find the row index where the headers are located
    //            int startRow = 1;
    //            int startColumn = 1;
    //            while (!worksheet.Cell(startRow, startColumn).Value.ToString()
    //                .Equals("Data movimento", StringComparison.OrdinalIgnoreCase))
    //            {
    //                startRow++;
    //            }

    //            // Iterate through each row starting from the headers
    //            for (int row = startRow + 1; row <= worksheet.LastRowUsed().RowNumber(); row++)
    //            {
    //                // Access data in each column for the current row
    //                IXLCell DataMovimentoCell = worksheet.Cell(row, startColumn);
    //                IXLCell DescrizioneOperazioneCell = worksheet.Cell(row, startColumn + 2);
    //                IXLCell CasualeCell = worksheet.Cell(row, startColumn + 3);
    //                IXLCell ImportoCell = worksheet.Cell(row, startColumn + 4);
    //                IXLCell DivisaCell = worksheet.Cell(row, startColumn + 5);

    //                response.Add(new SantanderMovements()
    //                {
    //                    DataMovimento = DataMovimentoCell.ToDateTime(),
    //                    DescrizioneOperazione = DescrizioneOperazioneCell.Value.ToString(),
    //                    Casuale = CasualeCell.Value.ToString(),
    //                    Divisa = DivisaCell.Value.ToString(),
    //                    Importo = ImportoCell.ToDecimal() ?? 0
    //                });
    //            }
    //        }
    //    }

    //    return response;
    //}

    public List<SantanderMovements> ReadMovements(IFormFile File)
    {
        List<SantanderMovements> response = new List<SantanderMovements>();

        using (var stream = new MemoryStream())
        {
            // Copy the content of the IFormFile into a MemoryStream
            File.CopyTo(stream);
            stream.Position = 0; // Reset the stream position

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Skip the first 8 rows
                for (int i = 0; i < 8; i++)
                {
                    reader.Read();
                }

                // Read the header row
                reader.Read();

                // Read the remaining rows as data
                while (reader.Read())
                {
                    response.Add(new SantanderMovements()
                    {
                        DataMovimento = ParseDateTime(reader.GetString(0)),
                        DescrizioneOperazione = reader.GetString(2),
                        Causale = reader.GetString(3),
                        Importo = Convert.ToDecimal(reader.GetDouble(4)),
                        Divisa = reader.GetString(5)
                    });
                }
            }
        }

        return response;
    }

    private DateTime ParseDateTime(string dateTimeString)
    {
        // Specify the expected date format
        string dateFormat = "dd/MM/yyyy";

        // Parse the string to DateTime using the specified format
        return DateTime.ParseExact(dateTimeString, dateFormat, CultureInfo.InvariantCulture);
    }

}
