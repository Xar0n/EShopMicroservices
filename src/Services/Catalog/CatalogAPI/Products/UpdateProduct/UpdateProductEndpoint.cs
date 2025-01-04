
using CatalogAPI.Products.GetProductById;

namespace CatalogAPI.Products.UpdateProduct;

public record UpdateProducRequest(Guid Id, string Name,
    List<string> Category, string Description,
    string ImageFile, decimal Price);

public record UpdateProducResponse(bool IsSucess);

public class UpdateProductEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products",
            async (UpdateProducRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProducResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Product")
            .WithDescription("Update Product");
    }
}
