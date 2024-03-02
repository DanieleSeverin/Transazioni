using Microsoft.AspNetCore.Http;
using Transazioni.Domain.Santander;

namespace Transazioni.Application.Santander.uploadSantanderMovements;

public interface ISantanderReader
{
    public List<SantanderMovements> ReadMovements(IFormFile File);
}
