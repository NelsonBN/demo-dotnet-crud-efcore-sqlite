using System;

namespace Demo.Api.Models;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    uint Quantity,
    double Price);
