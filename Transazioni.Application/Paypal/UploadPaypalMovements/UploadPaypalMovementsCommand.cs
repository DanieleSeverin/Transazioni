using Microsoft.AspNetCore.Http;
using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Paypal.UploadPaypalMovements;

public sealed record UploadPaypalMovementsCommand(IFormFile File, string AccountName, Guid UserId) : ICommand;
