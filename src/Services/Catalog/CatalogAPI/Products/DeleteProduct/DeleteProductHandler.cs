namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSucess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Product ID is required");
    }
}

internal class DeleteProductCommandHandler 
    (IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product == null) 
        { 
            throw new ProductNotFoundException(command.Id); 
        }

        session.Delete(product);
        await session.SaveChangesAsync();

        return new DeleteProductResult(true);
    }
}
