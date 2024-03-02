using Microsoft.AspNetCore.Http;
using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Santander.UploadSantanderMovements;

public sealed record UploadSantanderMovementsCommand(IFormFile File, string AccountName) : ICommand;
