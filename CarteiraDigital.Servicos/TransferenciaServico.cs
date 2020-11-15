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

        private readonly Action<Conta, decimal>[] realizarOperacaoPeloTipo;

        public TransferenciaServico(ITransferenciaRepositorio transferenciaRepositorio,
                                    IOperacaoServico operacaoServico,
                                    IContaServico contaServico,
                                    ITransacaoServico transacaoServico)
        {
            this.transferenciaRepositorio = transferenciaRepositorio;
            this.contaServico = contaServico;
            this.transacaoServico = transacaoServico;

            realizarOperacaoPeloTipo = new Action<Conta, decimal>[]
            {
                (conta, valor) => operacaoServico.Debitar(conta, valor),
                (conta, valor) => operacaoServico.Creditar(conta, valor)
            };
        }

        public void Efetivar(OperacaoBinariaDto dto)
        {
            using (var transacao = transacaoServico.GerarNova())
            {
                TransferirPeloTipo(dto.ContaOrigemId, dto.Valor, dto.Descricao, TipoTransferencia.Saida);
                TransferirPeloTipo(dto.ContaDestinoId, dto.Valor, dto.Descricao, TipoTransferencia.Entrada);

                transacao.Finalizar();
            }
        }

        public void TransferirPeloTipo(int contaId, decimal valor, string descricao,  TipoTransferencia tipoTransferencia)
        {
            var conta = contaServico.ObterConta(contaId);
            var transferencia = new Transferencia(valor, descricao, conta.Saldo, tipoTransferencia);

            realizarOperacaoPeloTipo[(int)tipoTransferencia](conta, valor);
            transferenciaRepositorio.Post(transferencia);

            contaServico.VincularTransferencia(conta, transferencia);
        }
    }
}
