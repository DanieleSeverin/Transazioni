using Microsoft.AspNetCore.Http;
using Transazioni.Domain.Fideuram;

namespace Transazioni.Application.Fideuram.UploadFideuramMovements;

public interface IFideuramReader
{
    public List<FideuramMovements> ReadMovements(IFormFile File);
}
