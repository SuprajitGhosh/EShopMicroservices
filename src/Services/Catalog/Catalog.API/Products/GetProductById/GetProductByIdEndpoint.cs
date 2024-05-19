﻿
using Catalog.API.Products.GetProduct;

namespace Catalog.API.Products.GetProductById
{
    //public record GetProductByIdRequest(Guid Guid);
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes( IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid Guid, ISender sender) => {
                var result = sender.Send(new GetProductByIdQuery(Guid));
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            })
                .WithName("GetProductById")
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product By Id")
                .WithDescription("Get Product By Id");
        }

    }
}
