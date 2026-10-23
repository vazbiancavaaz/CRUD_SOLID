using ProjetoSOLID.model;

namespace ProjetoSOLID.view;

public class ProdutoView : IProdutoView
{
    public void MostrarProduto(Produto produto)
    {
        Console.WriteLine($"ID: {produto.Id} | Nome: {produto.Nome} | Preço: {produto.Preco:C2}");
        Console.WriteLine($"Descrição: {produto.Descricao}");
        Console.WriteLine("----------------------------------------------");
    }
}
