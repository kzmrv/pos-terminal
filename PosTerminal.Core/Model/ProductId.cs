namespace PosTerminal.Core.Model
{
    public readonly struct ProductId
    {
        public ProductId(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
