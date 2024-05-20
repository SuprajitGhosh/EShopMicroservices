namespace Catalog.API.Products.GetProduct
{

    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 4) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);
    internal class GetProductQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 4, cancellationToken);
            return new GetProductsResult(products);
        }
    }
}
