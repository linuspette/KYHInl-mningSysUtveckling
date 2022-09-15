using WebApi.Repositories;

namespace WebApi.Helpers;

public interface ITokenReturnStatements
{
    public string TokenReturnStatement(ITokenHandler.StatusCode statusCode);
}
public class TokenReturnStatements : ITokenReturnStatements
{
    public string TokenReturnStatement(ITokenHandler.StatusCode statusCode)
    {
        switch (statusCode)
        {
            case ITokenHandler.StatusCode.Valid:
                return "Token is valid";
            case ITokenHandler.StatusCode.Expired:
                return "Session has expired";
            case ITokenHandler.StatusCode.NotAuthorized:
                return "You're not authorized to perform this action";
            case ITokenHandler.StatusCode.Failed:
                return "Token validation has failed";
            case ITokenHandler.StatusCode.Success:
                return "Token validation succeded";
        }

        return "";
    }
}