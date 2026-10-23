using Microsoft.Data.Sqlite;
using ProjetoSOLID.model;
using System.Globalization;

namespace ProjetoSOLID.dados;

public class CompraSqliteRepository : IRepository<Compra>
{
    public void Adicionar(Compra novo)
    {
        Validar(novo);
        using var conexao = BancoSqlite.AbrirConexao();
        using var transacao = conexao.BeginTransaction();

        using (var comando = conexao.CreateCommand())
        {
            comando.Transaction = transacao;
            comando.CommandText = "INSERT INTO Compras (Data) VALUES ($data); SELECT last_insert_rowid();";
            comando.Parameters.AddWithValue("$data", novo.Data.ToString("O", CultureInfo.InvariantCulture));
            novo.Id = Convert.ToInt32(comando.ExecuteScalar());
        }
        SalvarItens(conexao, transacao, novo);
        transacao.Commit();
    }

    public List<Compra> ListarTodos()
    {
        var lista = new List<Compra>();
        using var conexao = BancoSqlite.AbrirConexao();
        using (var comando = conexao.CreateCommand())
        {
            comando.CommandText = "SELECT Id, Data FROM Compras ORDER BY Id";
            using var leitor = comando.ExecuteReader();
            while (leitor.Read())
                lista.Add(LerCompra(leitor));
        }
        foreach (var compra in lista)
            compra.Itens = LerItens(conexao, compra.Id);
        return lista;
    }

    public Compra BuscarPorId(int id)
    {
        using var conexao = BancoSqlite.AbrirConexao();
        Compra compra;
        using (var comando = conexao.CreateCommand())
        {
            comando.CommandText = "SELECT Id, Data FROM Compras WHERE Id = $id";
            comando.Parameters.AddWithValue("$id", id);
            using var leitor = comando.ExecuteReader();
            compra = leitor.Read() ? LerCompra(leitor) : null;
        }
        if (compra != null)
            compra.Itens = LerItens(conexao, compra.Id);
        return compra;
    }

    public void Atualizar(Compra entidade)
    {
        Validar(entidade);
        using var conexao = BancoSqlite.AbrirConexao();
        using var transacao = conexao.BeginTransaction();
        using (var comando = conexao.CreateCommand())
        {
            comando.Transaction = transacao;
            comando.CommandText = "UPDATE Compras SET Data = $data WHERE Id = $id";
            comando.Parameters.AddWithValue("$data", entidade.Data.ToString("O", CultureInfo.InvariantCulture));
            comando.Parameters.AddWithValue("$id", entidade.Id);
            if (comando.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("Compra não encontrada.");
        }
        using (var comando = conexao.CreateCommand())
        {
            comando.Transaction = transacao;
            comando.CommandText = "DELETE FROM CompraItens WHERE CompraId = $id";
            comando.Parameters.AddWithValue("$id", entidade.Id);
            comando.ExecuteNonQuery();
        }
        SalvarItens(conexao, transacao, entidade);
        transacao.Commit();
    }

    public void Remover(Compra entidade) => Remover(entidade.Id);

    public void Remover(int id)
    {
        using var conexao = BancoSqlite.AbrirConexao();
        using var comando = conexao.CreateCommand();
        comando.CommandText = "DELETE FROM Compras WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    private static Compra LerCompra(SqliteDataReader leitor)
    {
        return new Compra
        {
            Id = leitor.GetInt32(0),
            Data = DateTime.Parse(leitor.GetString(1), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };
    }

    private static List<ItemCompra> LerItens(SqliteConnection conexao, int compraId)
    {
        var itens = new List<ItemCompra>();
        using var comando = conexao.CreateCommand();
        comando.CommandText = @"SELECT ProdutoId, NomeProduto, Quantidade, PrecoUnitario
                                  FROM CompraItens WHERE CompraId = $id ORDER BY ProdutoId";
        comando.Parameters.AddWithValue("$id", compraId);
        using var leitor = comando.ExecuteReader();
        while (leitor.Read())
        {
            itens.Add(new ItemCompra
            {
                ProdutoId = leitor.GetInt32(0),
                NomeProduto = leitor.GetString(1),
                Quantidade = leitor.GetInt32(2),
                PrecoUnitario = decimal.Parse(leitor.GetString(3), CultureInfo.InvariantCulture)
            });
        }
        return itens;
    }

    private static void SalvarItens(SqliteConnection conexao, SqliteTransaction transacao, Compra compra)
    {
        foreach (var item in compra.Itens)
        {
            using var comando = conexao.CreateCommand();
            comando.Transaction = transacao;
            comando.CommandText = @"INSERT INTO CompraItens
                (CompraId, ProdutoId, Quantidade, NomeProduto, PrecoUnitario)
                VALUES ($compra, $produto, $quantidade, $nome, $preco)";
            comando.Parameters.AddWithValue("$compra", compra.Id);
            comando.Parameters.AddWithValue("$produto", item.ProdutoId);
            comando.Parameters.AddWithValue("$quantidade", item.Quantidade);
            comando.Parameters.AddWithValue("$nome", item.NomeProduto);
            comando.Parameters.AddWithValue("$preco", item.PrecoUnitario.ToString(CultureInfo.InvariantCulture));
            comando.ExecuteNonQuery();
        }
    }

    private static void Validar(Compra compra)
    {
        if (compra == null || compra.Itens == null || compra.Itens.Count == 0)
            throw new InvalidOperationException("A compra precisa ter pelo menos um produto.");
        if (compra.Itens.Any(i => i.Quantidade <= 0 || i.PrecoUnitario < 0 || i.ProdutoId <= 0))
            throw new InvalidOperationException("A compra contém itens inválidos.");
        if (compra.Itens.Select(i => i.ProdutoId).Distinct().Count() != compra.Itens.Count)
            throw new InvalidOperationException("Um produto não pode ser repetido na mesma compra.");
    }
}
