using System.Threading.Tasks;

namespace Arta.Infrastructure.Validation
{
    public abstract class RequestValidator<TRequest> : IRequestValidator<TRequest>
    {
        public virtual int Order => 1;

        protected abstract ValidationResult Validate(TRequest request);

        public Task<ValidationResult> InternalValidate(TRequest request) => Task.FromResult(Validate(request));
    }
}