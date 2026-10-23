using System.Globalization;

namespace ProjetoSOLID.view;

public static class Leitura
{
    public static int Inteiro(string mensagem, int minimo)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (int.TryParse(Console.ReadLine(), out int valor) && valor >= minimo)
                return valor;
            Console.WriteLine($"Digite um número inteiro maior ou igual a {minimo}.");
        }
    }

    public static decimal DecimalNaoNegativo(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Number,
                CultureInfo.CurrentCulture, out decimal valor) && valor >= 0)
                return valor;
            Console.WriteLine("Informe um valor válido, por exemplo 19,90.");
        }
    }

    public static string TextoObrigatorio(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string texto = (Console.ReadLine() ?? "").Trim();
            if (texto.Length > 0)
                return texto;
            Console.WriteLine("Este campo não pode ficar vazio.");
        }
    }

    public static string TextoOpcional(string mensagem)
    {
        Console.Write(mensagem);
        return (Console.ReadLine() ?? "").Trim();
    }
}
