namespace Catalog.API.Products.GetProduct
{
    public record GetProductRequest(int? PageNumber = 1, int? PageSize = 4);
    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductRequest request, ISender sender) =>
            {
                var Query = request.Adapt<GetProductsQuery>();
                var result = await sender.Send(Query);
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(response);
            })
                .WithName("GetProduct")
                .Produces<GetProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product")
                .WithDescription("Get Product");
        }
    }
}
