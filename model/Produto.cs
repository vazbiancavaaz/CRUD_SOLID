namespace ProjetoSOLID.model;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; }

    public Produto(int id, string nome, decimal preco, string descricao)
    {
        Id = id;
        Nome = nome;
        Preco = preco;
        Descricao = descricao;
    }
}
