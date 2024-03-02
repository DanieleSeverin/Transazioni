using Microsoft.AspNetCore.Http;
using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Satispay.UploadSatispayMovements;

public sealed record UploadSatispayMovementsCommand(IFormFile File, string AccountName) : ICommand;