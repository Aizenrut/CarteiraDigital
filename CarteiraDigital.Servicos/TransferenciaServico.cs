using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos.Clients;
using System;
using System.Threading.Tasks;

namespace CarteiraDigital.Servicos
{
    public class TransferenciaServico : ITransferenciaServico
    {
        private readonly ITransferenciaRepositorio transferenciaRepositorio;
        private readonly IContaServico contaServico;
        private readonly ITransacaoServico transacaoServico;
        private readonly IOperacaoServico operacaoServico;
        private readonly IProdutorOperacoesClient produtorClient;

        private readonly Action<Conta, decimal>[] realizarOperacaoPeloTipo;

        public TransferenciaServico(ITransferenciaRepositorio transferenciaRepositorio,
                                    IOperacaoServico operacaoServico,
                                    IContaServico contaServico,
                                    ITransacaoServico transacaoServico,
                                    IProdutorOperacoesClient produtorClient)
        {
            this.transferenciaRepositorio = transferenciaRepositorio;
            this.contaServico = contaServico;
            this.transacaoServico = transacaoServico;
            this.operacaoServico = operacaoServico;
            this.produtorClient = produtorClient;

            realizarOperacaoPeloTipo = new Action<Conta, decimal>[]
            {
                (conta, valor) => operacaoServico.Debitar(conta, valor),
                (conta, valor) => operacaoServico.Creditar(conta, valor)
            };
        }

        public void ValidarContaDestino(int contaOrigemId, int contaDestinoId) 
        {
            if (contaOrigemId == contaDestinoId)
                throw new CarteiraDigitalException("Não é possível realizar uma transferência para a mesma conta!");
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

        public async Task Gerar(OperacaoBinariaDto dto)
        {
            ValidarContaDestino(dto.ContaOrigemId, dto.ContaDestinoId);
            operacaoServico.ValidarDescricao(dto.Descricao);

            int transferenciaSaidaId = GerarPeloTipo(dto.ContaOrigemId, dto.Valor, dto.Descricao, TipoMovimentacao.Saida);
            int transferenciaEntradaId = GerarPeloTipo(dto.ContaDestinoId, dto.Valor, dto.Descricao, TipoMovimentacao.Entrada);

            await produtorClient.EnfileirarTransferencia(new EfetivarOperacaoBinariaDto(transferenciaSaidaId, transferenciaEntradaId));
        }

        public int GerarPeloTipo(int contaId, decimal valor, string descricao, TipoMovimentacao tipoTransferencia)
        {
            var conta = contaServico.ObterConta(contaId);
            var transferencia = new Transferencia(contaId, valor, descricao, conta.Saldo, tipoTransferencia);

            transferenciaRepositorio.Post(transferencia);
            contaServico.VincularTransferencia(conta, transferencia);

            return transferencia.Id;
        }
    }
}
