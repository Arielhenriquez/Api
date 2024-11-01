using Response = Api.Application.Common.BaseResponse.BaseResponse;
namespace Api.Application.Common.Exceptions;


public class BadRequestException(string message) : Exception(message)
{
    public Response Response { get; } = Response.BadRequest(message);
}
