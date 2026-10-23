using ProjetoSOLID.model;

namespace ProjetoSOLID.view;

public interface ICadastroCompraView
{
    Compra ObterDados(IReadOnlyList<Produto> produtosDisponiveis);
    void ExibirMensagem(string mensagem);
}
