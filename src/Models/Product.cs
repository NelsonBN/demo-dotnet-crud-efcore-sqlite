using System;

namespace Demo.Api.Models;

public sealed class Product
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
    public uint Quantity { get; set; }
    public double Price { get; set; }
}
