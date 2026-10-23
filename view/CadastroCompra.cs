using ProjetoSOLID.model;

namespace ProjetoSOLID.view;

public class CadastroCompra : ICadastroCompraView
{
    public void ExibirMensagem(string mensagem) => Console.WriteLine(mensagem);

    public Compra ObterDados(IReadOnlyList<Produto> produtosDisponiveis)
    {
        var compra = new Compra();
        Console.WriteLine("===== PRODUTOS DISPONÍVEIS =====");
        foreach (var produto in produtosDisponiveis)
            Console.WriteLine($"ID {produto.Id}: {produto.Nome} | {produto.Preco:C2}");

        Console.WriteLine("Informe os itens da compra (digite 0 no ID para concluir).");
        while (true)
        {
            int id = Leitura.Inteiro("ID do produto: ", 0);
            if (id == 0)
                break;

            Produto produto = produtosDisponiveis.FirstOrDefault(p => p.Id == id);
            if (produto == null)
            {
                ExibirMensagem("Produto não encontrado.");
                continue;
            }
            if (compra.Itens.Any(i => i.ProdutoId == id))
            {
                ExibirMensagem("Produto já adicionado. Escolha outro ID.");
                continue;
            }
            int quantidade = Leitura.Inteiro("Quantidade: ", 1);
            compra.Itens.Add(new ItemCompra
            {
                ProdutoId = produto.Id,
                NomeProduto = produto.Nome,
                PrecoUnitario = produto.Preco,
                Quantidade = quantidade
            });
            Console.WriteLine($"Adicionado: {quantidade} x {produto.Nome}");
        }
        return compra;
    }
}
