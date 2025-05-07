namespace PropertyManager.API.Security.Domain
{
    public class ResultHash
    {
        public required string Hash { get; set; }
        public required byte[] Sal { get; set; }
    }
}
