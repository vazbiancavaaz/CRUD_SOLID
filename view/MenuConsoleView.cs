namespace ProjetoSOLID.view;

public class MenuConsoleView : IMenuView
{
    public int ExibirMenu()
    {
        Console.Clear();
        Console.WriteLine("===== GERENCIAMENTO DE PRODUTOS E COMPRAS =====");
        Console.WriteLine();
        Console.WriteLine(" 1 - Cadastrar produto");
        Console.WriteLine(" 2 - Listar produtos");
        Console.WriteLine(" 3 - Buscar produto");
        Console.WriteLine(" 4 - Atualizar produto");
        Console.WriteLine(" 5 - Excluir produto");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine(" 6 - Registrar compra");
        Console.WriteLine(" 7 - Listar compras");
        Console.WriteLine(" 8 - Buscar compra");
        Console.WriteLine(" 9 - Atualizar compra");
        Console.WriteLine("10 - Excluir compra");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine(" 0 - Sair");
        Console.WriteLine();
        Console.Write("Escolha uma opção: ");
        return int.TryParse(Console.ReadLine(), out int opcao) ? opcao : -1;
    }
}
