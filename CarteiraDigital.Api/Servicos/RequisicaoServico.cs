using CarteiraDigital.Servicos;

namespace CarteiraDigital.Api.Servicos
{
    public class RequisicaoServico : IRequisicaoServico
    {
        private readonly IContaServico contaServico;
        private readonly IJwtServico jwtServico;

        public RequisicaoServico(IContaServico contaServico,
                                 IJwtServico jwtServico)
        {
            this.contaServico = contaServico;
            this.jwtServico = jwtServico;
        }

        public int ObterContaDoCliente(string token)
        {
            var usuario = jwtServico.ObterSubject(token.Replace("Bearer ", ""));
            return contaServico.ObterIdPeloTitular(usuario);
        }
    }
}
