using ProjetoSOLID.dados;
using ProjetoSOLID.model;
using ProjetoSOLID.view;

namespace ProjetoSOLID.controller;

public class ProdutoController
{
    private readonly IRepository<Produto> repository;
    private readonly IProdutoView view;
    private readonly ICadastroView<Produto> cadastroView;

    public ProdutoController(IProdutoView view, IRepository<Produto> repository, ICadastroView<Produto> cadastroView)
    {
        this.view = view;
        this.repository = repository;
        this.cadastroView = cadastroView;
    }

    public void Cadastrar()
    {
        Produto produto = cadastroView.ObterDados();
        repository.Adicionar(produto);
        cadastroView.ExibirMensagem($"Produto cadastrado com sucesso! ID: {produto.Id}");
    }

    public void Listar()
    {
        var produtos = repository.ListarTodos();
        if (produtos.Count == 0)
            cadastroView.ExibirMensagem("Nenhum produto cadastrado.");
        foreach (var produto in produtos)
            view.MostrarProduto(produto);
    }

    public Produto Buscar(int id) => repository.BuscarPorId(id);

    public void Atualizar(Produto produto) => repository.Atualizar(produto);

    public void Atualizar(int id)
    {
        if (Buscar(id) == null)
        {
            cadastroView.ExibirMensagem("Produto não encontrado.");
            return;
        }
        Produto alterado = cadastroView.ObterDados();
        alterado.Id = id;
        repository.Atualizar(alterado);
        cadastroView.ExibirMensagem("Produto atualizado com sucesso!");
    }

    public void Remover(Produto produto) => Remover(produto.Id);

    public void Remover(int id)
    {
        if (Buscar(id) == null)
        {
            cadastroView.ExibirMensagem("Produto não encontrado.");
            return;
        }
        repository.Remover(id);
        cadastroView.ExibirMensagem("Produto excluído com sucesso!");
    }
}
