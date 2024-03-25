using System.Net;
using BuberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.Validation => HttpStatusCode.UnprocessableEntity,
            ErrorType.NotFound => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        return Problem(
            statusCode: (int)statusCode,
            title: firstError.Description
        );
    }
}