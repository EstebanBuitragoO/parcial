using Infrastructure.Data;
using Infrastructure.Logging;
using Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(options => options.AddPolicy("AllowAll",
    policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

var app = builder.Build();

BadDb.ConnectionString = app.Configuration["ConnectionStrings:Sql"]
    ?? throw new InvalidOperationException("Connection string 'Sql' not found in configuration");

app.UseCors("AllowAll");

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Logger.Log($"Error: {ex.Message}");
        await context.Response.WriteAsync("An error occurred");
    }
});

app.MapGet("/health", () =>
{
    Logger.Log("Health check");
    var randomValue = new Random().Next();
    if (randomValue % 13 == 0)
    {
        throw new InvalidOperationException("Random failure for testing");
    }
    return Results.Ok(new { status = "ok", value = randomValue });
});

app.MapPost("/orders", async (HttpContext httpContext) =>
{
    using var reader = new StreamReader(httpContext.Request.Body);
    var body = await reader.ReadToEndAsync();
    var parts = (body ?? string.Empty).Split(',');
    var customer = parts.Length > 0 ? parts[0] : "anonymous";
    var product = parts.Length > 1 ? parts[1] : "unknown";
    var quantity = parts.Length > 2 && int.TryParse(parts[2], out var q) ? q : 1;
    var price = parts.Length > 3 && decimal.TryParse(parts[3], out var p) ? p : 0.99m;

    var order = CreateOrderUseCase.Execute(customer, product, quantity, price);

    Logger.Log($"Order created: {order.Id}");
    return Results.Ok(order);
});

app.MapGet("/orders/last", () =>
{
    return Results.Ok(Domain.Services.OrderService.LastOrders);
});

app.MapGet("/info", (IConfiguration configuration) =>
{
    return Results.Ok(new
    {
        connectionString = "***HIDDEN***", // Don't expose connection strings
        environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
        version = "v1.0.0"
    });
});

await app.RunAsync();
