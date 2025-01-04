using Microsoft.Extensions.Logging;

namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSucess);


internal class DeleteProductCommandHandler 
    (IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);

        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product == null) 
        { 
            throw new ProductNotFoundException(); 
        }

        session.Delete(product);
        await session.SaveChangesAsync();

        return new DeleteProductResult(true);
    }
}
