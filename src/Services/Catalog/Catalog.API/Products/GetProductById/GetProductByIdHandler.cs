namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid guId) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    internal class GetProductByIdQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        async public Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(query.guId, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(query.guId);
            }
            return new GetProductByIdResult(product);
        }
    }
}
