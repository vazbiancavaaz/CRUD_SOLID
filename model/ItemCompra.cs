namespace ProjetoSOLID.model;

// Classe de associação entre Compra e Produto no relacionamento N:N.
public class ItemCompra
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = "";
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;
}
