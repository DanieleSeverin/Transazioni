using Microsoft.AspNetCore.Http;
using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Fideuram.UploadFideuramMovements;

public sealed record UploadFideuramMovementsCommand(IFormFile File, string AccountName, Guid UserId) : ICommand;
