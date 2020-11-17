using System;
using System.Linq.Expressions;

namespace CarteiraDigital.Dados.Repositorios
{
    public interface IRepositorio<T>
    {
        bool Any(int id);
        bool Any(Expression<Func<T, bool>> expression);
        T Get(int id);
        void Post(T entidade);
        void Update(T entidade);
        void Update(params T[] entidades);
    }
}
