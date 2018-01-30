using System.Threading.Tasks;

namespace Arta.Infrastructure.Validation
{
    public abstract class AsyncRequestValidator<TRequest> : IRequestValidator<TRequest>
    {
        public virtual int Order => 1;

        protected abstract Task<ValidationResult> Validate(TRequest request);

        public Task<ValidationResult> InternalValidate(TRequest request) => Validate(request);
    }
}