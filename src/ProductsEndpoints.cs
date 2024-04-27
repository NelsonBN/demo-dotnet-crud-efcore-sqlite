using System;
using System.Linq;
using System.Threading;
using Demo.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/products", async (AppDbContext context, CancellationToken cancellationToken) =>
        {
            var products = await context.Products.ToListAsync(cancellationToken);

            products.Select(s => new ProductResponse(
                s.Id,
                s.Name,
                s.Quantity,
                s.Price));

            return Results.Ok(products);
        });


        endpoints.MapGet("/products/{id}", async (AppDbContext context, Guid id, CancellationToken cancellationToken) =>
        {
            var product = await context.Products.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (product is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(new ProductResponse(
                product.Id,
                product.Name,
                product.Quantity,
                product.Price));
        }).WithName("GetProduct");


        endpoints.MapPost("/products", async (AppDbContext context, ProductRequest request, CancellationToken cancellationToken) =>
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Quantity = request.Quantity,
                Price = request.Price
            };

            context.Products.Add(product);
            await context.SaveChangesAsync(cancellationToken);

            return Results.CreatedAtRoute(
                "GetProduct",
                new { product.Id },
                product.Id);
        });


        endpoints.MapPut("/products/{id}", async (AppDbContext context, Guid id, ProductRequest request, CancellationToken cancellationToken) =>
        {
            var product = await context.Products.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (product is null)
            {
                return Results.NotFound();
            }

            product.Name = request.Name;
            product.Quantity = request.Quantity;
            product.Price = request.Price;

            context.Products.Update(product);
            await context.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        });


        endpoints.MapDelete("/products/{id}", async (AppDbContext context, Guid id, CancellationToken cancellationToken) =>
        {
            if (!await context.Products.AnyAsync(s => s.Id == id, cancellationToken))
            {
                return Results.NotFound();
            }

            await context.Products.Where(s => s.Id == id).ExecuteDeleteAsync(cancellationToken);
            return Results.NoContent();
        });
    }
}
