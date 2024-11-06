namespace TP6.Models
{
    public class Producto
    {
        private int _idProduct;
        private string description;
        private int price;

        public Producto()
        {
        }

        public Producto(int idProduct, string description, int price)
        {
            _idProduct = idProduct;
            this.description = description;
            this.price = price;
        }

        public int IdProduct { get => _idProduct; set => _idProduct = value; }
        public string Description { get => description; set => description = value; }
        public int Price { get => price; set => price = value; }
    }
}
