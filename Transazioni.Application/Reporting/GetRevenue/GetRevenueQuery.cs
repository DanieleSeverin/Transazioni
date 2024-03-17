using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Reporting.GetRevenue;

public sealed record GetRevenueQuery : IQuery<List<RevenueSummary>>;
