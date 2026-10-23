using Microsoft.Data.Sqlite;

namespace ProjetoSOLID.dados; 

public static class BancoSqlite
{
    // O banco será criado na pasta de execução do aplicativo.
    private const string StringConexao = "Data Source=projeto_solid.db;Foreign Keys=True";

    public static SqliteConnection AbrirConexao()
    {
        var conexao = new SqliteConnection(StringConexao);
        conexao.Open();
        return conexao;
    }

    public static void Inicializar()
    {
        using var conexao = AbrirConexao();
        using var comando = conexao.CreateCommand();
        comando.CommandText = @"
            CREATE TABLE IF NOT EXISTS Produtos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Preco TEXT NOT NULL,
                Descricao TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Compras (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Data TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS CompraItens (
                CompraId INTEGER NOT NULL,
                ProdutoId INTEGER NOT NULL,
                Quantidade INTEGER NOT NULL CHECK (Quantidade > 0),
                NomeProduto TEXT NOT NULL,
                PrecoUnitario TEXT NOT NULL,
                PRIMARY KEY (CompraId, ProdutoId),
                FOREIGN KEY (CompraId) REFERENCES Compras(Id) ON DELETE CASCADE,
                FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id) ON DELETE RESTRICT
            );";
        comando.ExecuteNonQuery();
    }
}
