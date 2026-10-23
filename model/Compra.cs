namespace ProjetoSOLID.model;

public class Compra
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;
    public List<ItemCompra> Itens { get; set; } = new();
    public decimal Total => Itens.Sum(item => item.Subtotal);
}
