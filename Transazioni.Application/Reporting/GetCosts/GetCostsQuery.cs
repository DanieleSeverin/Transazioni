using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Reporting.GetCosts;

public sealed record GetCostsQuery : IQuery<List<CostsSummary>>;
