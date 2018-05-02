using Microsoft.AspNetCore.Mvc;

namespace Arta.Infrastructure
{
    public static class ApiResultToActionResultMapper
    {
        public static IActionResult ToActionResult(ApiResult result)
        {
            return result.IsSuccessful
                ? new ObjectResult(null)
                    { StatusCode = (int)result.HttpStatusCode }

                : result.ErrorCode == null
                    ? new ObjectResult(new Error { ErrorDescription = result.ErrorDescription })
                        { StatusCode = result.HttpStatusCode }
                    : new ObjectResult(new ErrorWithErrorCode { ErrorDescription = result.ErrorDescription, ErrorCode = result.ErrorCode })
                        { StatusCode = result.HttpStatusCode };
        }

        public static IActionResult ToActionResult<TResponse>(ApiResult<TResponse> result)
        {
            return result.IsSuccessful
                ? new ObjectResult(result.Value)
                    { StatusCode = (int)result.HttpStatusCode }

                : result.ErrorCode == null
                    ? new ObjectResult(new Error { ErrorDescription = result.ErrorDescription })
                        { StatusCode = (int)result.HttpStatusCode }
                    : new ObjectResult(new ErrorWithErrorCode() { ErrorDescription = result.ErrorDescription, ErrorCode = result.ErrorCode })
                        { StatusCode = (int)result.HttpStatusCode };
        }
    }
}