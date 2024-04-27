namespace Demo.Api.Models;

public sealed record ProductRequest(
    string Name,
    uint Quantity,
    double Price);
