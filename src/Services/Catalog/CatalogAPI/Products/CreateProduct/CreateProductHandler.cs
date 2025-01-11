using System.Data;

namespace CatalogAPI.Products.CreateProduct;

public record CreateProductCommand(string Name,
    List<string> Category, string Description, 
    string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator 
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(n => n.Name)
            .NotEmpty().WithMessage("Name is required");
        RuleFor(n => n.Category)
            .NotEmpty().WithMessage("Category is required");
        RuleFor(n => n.Description)
            .NotEmpty().WithMessage("Description is required");
        RuleFor(n => n.ImageFile)
            .NotEmpty().WithMessage("ImageFile is required");
        RuleFor(n => n.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

    }
}

internal class CreateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
