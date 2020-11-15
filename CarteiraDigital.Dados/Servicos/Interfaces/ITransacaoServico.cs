using System;

namespace CarteiraDigital.Dados.Servicos
{
    public interface ITransacaoServico : IDisposable
    {
        ITransacaoServico GerarNova();
        void Finalizar();
    }
}
