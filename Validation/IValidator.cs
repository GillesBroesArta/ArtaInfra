namespace AArta.nfrastructure.Validation
{
    public interface IValidator<in TRequest>
    {
        ValidationResult Validate(TRequest request);
        int Order { get; }
    }
}