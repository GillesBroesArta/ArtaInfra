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

                : new ObjectResult(new Error { ErrorMessage = result.ErrorMessage })
                    { StatusCode = (int)result.HttpStatusCode };
        }

        public static IActionResult ToActionResult<TResponse>(ApiResult<TResponse> result)
        {
            return result.IsSuccessful
                ? new ObjectResult(result.Value)
                    { StatusCode = (int)result.HttpStatusCode }

                : result.ErrorCode == null
                    ? new ObjectResult(new Error { ErrorMessage = result.ErrorMessage })
                        { StatusCode = (int)result.HttpStatusCode }
                    : new ObjectResult(new ErrorWithErrorCode() { ErrorMessage = result.ErrorMessage, ErrorCode = result.ErrorCode })
                        { StatusCode = (int)result.HttpStatusCode };
        }
    }
}