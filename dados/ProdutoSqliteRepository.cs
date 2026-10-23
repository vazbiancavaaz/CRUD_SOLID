using Microsoft.Data.Sqlite;
using ProjetoSOLID.model;
using System.Globalization;

namespace ProjetoSOLID.dados;

public class ProdutoSqliteRepository : IRepository<Produto>
{
    public void Adicionar(Produto novo)
    {
        using var conexao = BancoSqlite.AbrirConexao();
        using var comando = conexao.CreateCommand();
        comando.CommandText = @"
            INSERT INTO Produtos (Nome, Preco, Descricao)
            VALUES ($nome, $preco, $descricao);
            SELECT last_insert_rowid();";
        PreencherParametros(comando, novo);
        novo.Id = Convert.ToInt32(comando.ExecuteScalar());
    }

    public List<Produto> ListarTodos()
    {
        var lista = new List<Produto>();
        using var conexao = BancoSqlite.AbrirConexao();
        using var comando = conexao.CreateCommand();
        comando.CommandText = "SELECT Id, Nome, Preco, Descricao FROM Produtos ORDER BY Id";
        using var leitor = comando.ExecuteReader();
        while (leitor.Read())
            lista.Add(LerProduto(leitor));
        return lista;
    }

    public Produto BuscarPorId(int id)
    {
        using var conexao = BancoSqlite.AbrirConexao();
        using var comando = conexao.CreateCommand();
        comando.CommandText = "SELECT Id, Nome, Preco, Descricao FROM Produtos WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        using var leitor = comando.ExecuteReader();
        return leitor.Read() ? LerProduto(leitor) : null;
    }

    public void Atualizar(Produto entidade)
    {
        using var conexao = BancoSqlite.AbrirConexao();
        using var comando = conexao.CreateCommand();
        comando.CommandText = @"
            UPDATE Produtos SET Nome = $nome, Preco = $preco, Descricao = $descricao
            WHERE Id = $id";
        PreencherParametros(comando, entidade);
        comando.Parameters.AddWithValue("$id", entidade.Id);
        comando.ExecuteNonQuery();
    }

    public void Remover(Produto entidade) => Remover(entidade.Id);

    public void Remover(int id)
    {
        using var conexao = BancoSqlite.AbrirConexao();
        using var comando = conexao.CreateCommand();
        comando.CommandText = "DELETE FROM Produtos WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    private static Produto LerProduto(SqliteDataReader leitor)
    {
        return new Produto(
            leitor.GetInt32(0),
            leitor.GetString(1),
            decimal.Parse(leitor.GetString(2), CultureInfo.InvariantCulture),
            leitor.GetString(3));
    }

    private static void PreencherParametros(SqliteCommand comando, Produto produto)
    {
        comando.Parameters.AddWithValue("$nome", produto.Nome);
        comando.Parameters.AddWithValue("$preco", produto.Preco.ToString(CultureInfo.InvariantCulture));
        comando.Parameters.AddWithValue("$descricao", produto.Descricao);
    }
}
