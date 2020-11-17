using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CarteiraDigital.Dados.Repositorios
{
    public abstract class Repositorio<T> where T : Entidade
    {
        protected readonly CarteiraDigitalContext context;

        public Repositorio(CarteiraDigitalContext context)
        {
            this.context = context;
        }

        public void BeginTransaction()
        {
            context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            context.Database.CommitTransaction();
        }

        public bool Any(int id)
        {
            return context.Set<T>().Any(x => x.Id == id);
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().Any(expression);
        }

        public T Get(int id)
        {
            return context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public void Post(T entidade)
        {
            context.Set<T>().Add(entidade);
            context.SaveChanges();
        }

        public void Update(T entidade)
        {
            context.Set<T>().Update(entidade);
            context.SaveChanges();
        }

        public void Update(params T[] entidades)
        {
            context.Set<T>().UpdateRange(entidades);
            context.SaveChanges();
        }
    }
}
