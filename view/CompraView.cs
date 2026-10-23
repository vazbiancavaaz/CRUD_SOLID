using ProjetoSOLID.model;

namespace ProjetoSOLID.view;

public class CompraView : ICompraView
{
    public void MostrarCompra(Compra compra)
    {
        Console.WriteLine($"Compra #{compra.Id} | Data: {compra.Data:dd/MM/yyyy HH:mm}");
        foreach (var item in compra.Itens)
        {
            Console.WriteLine($"  Produto #{item.ProdutoId}: {item.NomeProduto} | " +
                $"Qtd.: {item.Quantidade} x {item.PrecoUnitario:C2} = {item.Subtotal:C2}");
        }
        Console.WriteLine($"TOTAL: {compra.Total:C2}");
        Console.WriteLine("----------------------------------------------");
    }
}
