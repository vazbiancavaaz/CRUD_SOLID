using ProjetoSOLID.dados;
using ProjetoSOLID.model;
using ProjetoSOLID.view;

namespace ProjetoSOLID.controller;

public class CompraController
{
    private readonly IRepository<Compra> compras;
    private readonly IRepository<Produto> produtos;
    private readonly ICompraView view;
    private readonly ICadastroCompraView cadastroView;

    public CompraController(IRepository<Compra> compras, IRepository<Produto> produtos,
        ICompraView view, ICadastroCompraView cadastroView)
    {
        this.compras = compras;
        this.produtos = produtos;
        this.view = view;
        this.cadastroView = cadastroView;
    }

    public void Cadastrar()
    {
        var produtosDisponiveis = produtos.ListarTodos();
        if (produtosDisponiveis.Count == 0)
        {
            cadastroView.ExibirMensagem("Cadastre um produto antes de registrar uma compra.");
            return;
        }
        Compra compra = cadastroView.ObterDados(produtosDisponiveis);
        if (compra.Itens.Count == 0)
        {
            cadastroView.ExibirMensagem("Compra cancelada: nenhum item informado.");
            return;
        }
        ValidarProdutos(compra);
        compras.Adicionar(compra);
        cadastroView.ExibirMensagem($"Compra #{compra.Id} registrada com sucesso!");
        view.MostrarCompra(compra);
    }

    public void Listar()
    {
        var lista = compras.ListarTodos();
        if (lista.Count == 0)
            cadastroView.ExibirMensagem("Nenhuma compra registrada.");
        foreach (var compra in lista)
            view.MostrarCompra(compra);
    }

    public void Mostrar(int id)
    {
        Compra compra = compras.BuscarPorId(id);
        if (compra == null)
            cadastroView.ExibirMensagem("Compra não encontrada.");
        else
            view.MostrarCompra(compra);
    }

    public void Atualizar(int id)
    {
        Compra atual = compras.BuscarPorId(id);
        if (atual == null)
        {
            cadastroView.ExibirMensagem("Compra não encontrada.");
            return;
        }
        var disponiveis = produtos.ListarTodos();
        if (disponiveis.Count == 0)
        {
            cadastroView.ExibirMensagem("Não existem produtos disponíveis.");
            return;
        }
        cadastroView.ExibirMensagem("Informe novamente os itens desta compra:");
        Compra alterada = cadastroView.ObterDados(disponiveis);
        if (alterada.Itens.Count == 0)
        {
            cadastroView.ExibirMensagem("Atualização cancelada: nenhum item informado.");
            return;
        }
        alterada.Id = id;
        alterada.Data = atual.Data;
        ValidarProdutos(alterada);
        compras.Atualizar(alterada);
        cadastroView.ExibirMensagem("Compra atualizada com sucesso!");
        view.MostrarCompra(alterada);
    }

    public void Remover(int id)
    {
        if (compras.BuscarPorId(id) == null)
        {
            cadastroView.ExibirMensagem("Compra não encontrada.");
            return;
        }
        compras.Remover(id);
        cadastroView.ExibirMensagem("Compra excluída com sucesso!");
    }

    private void ValidarProdutos(Compra compra)
    {
        foreach (var item in compra.Itens)
        {
            Produto produto = produtos.BuscarPorId(item.ProdutoId);
            if (produto == null)
                throw new InvalidOperationException($"Produto com ID {item.ProdutoId} não existe.");
            if (item.Quantidade <= 0)
                throw new InvalidOperationException("A quantidade deve ser positiva.");
            item.NomeProduto = produto.Nome;
            item.PrecoUnitario = produto.Preco;
        }
    }
}
