namespace ChampionsChromo.Core.Models
{
    public class CreatePaymentIntentRequest
    {
        public long Amount { get; set; }

        public string Currency { get; set; } = "brl";

        public IList<CartItem>? Items { get; set; }
    }

    public class CartItem
    {
        public AlbumDto Album { get; set; } = new();
        public IList<Sticker> Stickers { get; set; } = [];
    }

    public class Sticker
    {
        public string Id { get; set; }
        public string AlbumId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
    }
}
