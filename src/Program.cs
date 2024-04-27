using Demo.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .AddDbContext<AppDbContext>();


var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI();

app.MapProductsEndpoints();

app.Run();

public partial class Program;
