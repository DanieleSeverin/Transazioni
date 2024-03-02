using Microsoft.AspNetCore.Http;
using Transazioni.Domain.Paypal;

namespace Transazioni.Application.Paypal.UploadPaypalMovements;

public interface IPaypalReader
{
    public List<PaypalMovements> ReadMovements(IFormFile File);

}
