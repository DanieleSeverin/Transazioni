using Microsoft.AspNetCore.Http;
using Transazioni.Domain.Satispay;

namespace Transazioni.Application.Satispay.UploadSatispayMovements;

public interface ISatispayReader
{
    public List<SatispayMovements> ReadMovements(IFormFile File);
}
