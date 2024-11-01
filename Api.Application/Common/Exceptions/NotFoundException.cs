using Response = Api.Application.Common.BaseResponse.BaseResponse;
namespace Api.Application.Common.Exceptions;


public class NotFoundException : Exception
{
    public Response Response { get; }

    public NotFoundException(string message)
      : base(message)
    {
        Response = Response.NotFound(message);
    }

    public NotFoundException(string entityName, Guid id)
      : base($"{entityName} with id: {id} not found")
    {
        Response = Response.NotFound(Message);
    }
}
