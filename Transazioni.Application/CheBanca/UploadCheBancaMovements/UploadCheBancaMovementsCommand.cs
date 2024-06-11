using Microsoft.AspNetCore.Http;
using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.CheBanca.UploadCheBancaMovements;

public sealed record UploadCheBancaMovementsCommand(IFormFile File, string AccountName, Guid UserId) : ICommand;
