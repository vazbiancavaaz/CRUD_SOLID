using Microsoft.Data.Sqlite;
using ProjetoSOLID.controller;
using ProjetoSOLID.dados;
using ProjetoSOLID.model;
using ProjetoSOLID.view;
using System.Globalization;
using System.Text;

namespace ProjetoSOLID;

internal class Program
{
    private static void Main(string[] args)
    {
        CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
        CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");
        Console.OutputEncoding = Encoding.UTF8;

        BancoSqlite.Inicializar();

        IRepository<Produto> produtos = new ProdutoSqliteRepository();
        IRepository<Compra> compras = new CompraSqliteRepository();

        IProdutoView produtoView = new ProdutoView();
        ICadastroView<Produto> cadastroProdutoView = new CadastroProduto();
        ICadastroCompraView cadastroCompraView = new CadastroCompra();
        ICompraView compraView = new CompraView();
        IMenuView menuView = new MenuConsoleView();

        var produtoController = new ProdutoController(produtoView, produtos, cadastroProdutoView);
        var compraController = new CompraController(compras, produtos, compraView, cadastroCompraView);

        int opcao;
        do
        {
            opcao = menuView.ExibirMenu();
            Console.Clear();
            try
            {
                switch (opcao)
                {
                    case 1:
                        produtoController.Cadastrar();
                        break;
                    case 2:
                        produtoController.Listar();
                        break;
                    case 3:
                        Produto buscado = produtoController.Buscar(Leitura.Inteiro("ID do produto: ", 1));
                        if (buscado == null)
                            Console.WriteLine("Produto não encontrado.");
                        else
                            produtoView.MostrarProduto(buscado);
                        break;
                    case 4:
                        produtoController.Atualizar(Leitura.Inteiro("ID do produto a atualizar: ", 1));
                        break;
                    case 5:
                        produtoController.Remover(Leitura.Inteiro("ID do produto a excluir: ", 1));
                        break;
                    case 6:
                        compraController.Cadastrar();
                        break;
                    case 7:
                        compraController.Listar();
                        break;
                    case 8:
                        compraController.Mostrar(Leitura.Inteiro("ID da compra: ", 1));
                        break;
                    case 9:
                        compraController.Atualizar(Leitura.Inteiro("ID da compra a atualizar: ", 1));
                        break;
                    case 10:
                        compraController.Remover(Leitura.Inteiro("ID da compra a excluir: ", 1));
                        break;
                    case 0:
                        Console.WriteLine("Encerrando o sistema...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
            catch (SqliteException ex) when (ex.SqliteExtendedErrorCode == 787)
            {
                Console.WriteLine("Não foi possível excluir o produto porque ele já aparece em compras registradas.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Não foi possível concluir a operação: {ex.Message}");
            }

            if (opcao != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
            }
        } while (opcao != 0);
    }
}
