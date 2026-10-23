using ProjetoSOLID.model;

namespace ProjetoSOLID.view;

public class CadastroProduto : ICadastroView<Produto>
{
    public void ExibirMensagem(string mensagem) => Console.WriteLine(mensagem);

    public Produto ObterDados()
    {
        string nome = Leitura.TextoObrigatorio("Nome: ");
        decimal preco = Leitura.DecimalNaoNegativo("Preço (R$): ");
        string descricao = Leitura.TextoOpcional("Descrição: ");
        // ID zero indica cadastro novo; o SQLite atribuirá um ID único.
        return new Produto(0, nome, preco, descricao);
    }
}
