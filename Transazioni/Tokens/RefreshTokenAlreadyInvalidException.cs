namespace Transazioni.Domain.Tokens;

public class RefreshTokenAlreadyInvalidException : Exception 
{ 
    public RefreshTokenAlreadyInvalidException(RefreshTokenId Id) 
        : base($"Refresh Token Id: {Id.Value.ToString()}") { }
    
}
