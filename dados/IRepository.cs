using System.Collections.Generic;

namespace ProjetoSOLID.dados;

// Interface genérica reutilizada nos repositórios de produtos e compras.
public interface IRepository<T>
{
    void Adicionar(T novo);
    List<T> ListarTodos();
    T BuscarPorId(int id);
    void Atualizar(T entidade);
    void Remover(T entidade);
    void Remover(int id);
}
