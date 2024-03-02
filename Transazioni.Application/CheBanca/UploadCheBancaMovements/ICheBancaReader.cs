using Microsoft.AspNetCore.Http;
using Transazioni.Domain.CheBanca;

namespace Transazioni.Application.CheBanca.UploadCheBancaMovements;

public interface ICheBancaReader
{
    public List<CheBancaMovements> ReadMovements(IFormFile File);
}
