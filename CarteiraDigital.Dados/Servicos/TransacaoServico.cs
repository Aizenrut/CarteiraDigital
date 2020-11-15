using CarteiraDigital.Dados.Contexts;

namespace CarteiraDigital.Dados.Servicos
{
    public class TransacaoServico : ITransacaoServico
    {
        private readonly CarteiraDigitalContext context;
        private bool finalizada;

        public TransacaoServico(CarteiraDigitalContext context)
        {
            this.context = context;
        }

        public ITransacaoServico GerarNova()
        {
            context.Database.BeginTransaction();
            return this;
        }

        public void Finalizar()
        {
            finalizada = true;
        }

        public void Dispose()
        {
            if (finalizada)
                context.Database.CommitTransaction();
            else
                context.Database.RollbackTransaction();
        }
    }
}
