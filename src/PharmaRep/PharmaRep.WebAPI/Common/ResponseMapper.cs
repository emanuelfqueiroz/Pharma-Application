using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Application.Common;

namespace _PharmaRep.WebAPI.Common;

public static class ResponseMapper
{
    public static IActionResult Match<TValue>(this CommandResponse<TValue> response,
        Func<TValue, IActionResult> onSuccess,
        Func<string, IActionResult> onError) => response switch
        {
            { IsSuccess: true, Data: null } => new NotFoundResult(),
            { IsSuccess: true } => onSuccess is null ? new OkObjectResult(response.Data) : onSuccess(response.Data!),
            { IsSuccess: false } => onError(response.Message!),
            _ => throw new NotImplementedException()
        };
}
