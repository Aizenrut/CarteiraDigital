using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public class TransferenciaServico : ITransferenciaServico
    {
        private readonly ITransferenciaRepositorio transferenciaRepositorio;
        private readonly IContaServico contaServico;
        private readonly ITransacaoServico transacaoServico;
        private readonly IOperacaoServico operacaoServico;


        private readonly Action<Conta, decimal>[] realizarOperacaoPeloTipo;

        public TransferenciaServico(ITransferenciaRepositorio transferenciaRepositorio,
                                    IOperacaoServico operacaoServico,
                                    IContaServico contaServico,
                                    ITransacaoServico transacaoServico)
        {
            this.transferenciaRepositorio = transferenciaRepositorio;
            this.contaServico = contaServico;
            this.transacaoServico = transacaoServico;
            this.operacaoServico = operacaoServico;

            realizarOperacaoPeloTipo = new Action<Conta, decimal>[]
            {
                (conta, valor) => operacaoServico.Debitar(conta, valor),
                (conta, valor) => operacaoServico.Creditar(conta, valor)
            };
        }

        public void Efetivar(EfetivarOperacaoBinariaDto dto)
        {
            var transferenciaSaida = transferenciaRepositorio.Get(dto.OperacaoSaidaId);
            var transferenciaEntrada = transferenciaRepositorio.Get(dto.OperacaoEntradaId);

            try
            {
                using (var transacao = transacaoServico.GerarNova())
                {
                    Efetivar(transferenciaSaida);
                    Efetivar(transferenciaEntrada);

                    transacao.Finalizar();
                }
            }
            catch (CarteiraDigitalException e)
            {
                operacaoServico.MarcarComErro(transferenciaSaida, e.Message);
                operacaoServico.MarcarComErro(transferenciaEntrada, e.Message);
            }

            transferenciaRepositorio.Update(transferenciaSaida, transferenciaEntrada);
        }

        public void Efetivar(Transferencia transferencia)
        {
            var conta = contaServico.ObterConta(transferencia.ContaId);

            realizarOperacaoPeloTipo[(int)transferencia.TipoMovimentacao](conta, transferencia.Valor);
            operacaoServico.MarcarEfetivada(transferencia);
        }

        public void Gerar(OperacaoBinariaDto dto)
        {
            GerarPeloTipo(dto.ContaOrigemId, dto.Valor, dto.Descricao, TipoMovimentacao.Saida);
            GerarPeloTipo(dto.ContaDestinoId, dto.Valor, dto.Descricao, TipoMovimentacao.Entrada);
        }

        public void GerarPeloTipo(int contaId, decimal valor, string descricao, TipoMovimentacao tipoTransferencia)
        {
            var conta = contaServico.ObterConta(contaId);
            var transferencia = new Transferencia(contaId, valor, descricao, conta.Saldo, tipoTransferencia);

            operacaoServico.MarcarPendente(transferencia);

            transferenciaRepositorio.Post(transferencia);
            contaServico.VincularTransferencia(conta, transferencia);
        }
    }
}
