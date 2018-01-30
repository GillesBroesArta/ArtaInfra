using System.Threading.Tasks;

namespace Arta.Infrastructure.Validation
{
    public interface IRequestValidator<in TRequest>
    {
        Task<ValidationResult> InternalValidate(TRequest request);
        int Order { get; }
    }
}