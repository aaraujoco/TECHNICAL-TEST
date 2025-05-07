namespace PropertyManager.API.Security.Domain.Model
{
    public class ResponseAutenticationModel
    {
        public required string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
