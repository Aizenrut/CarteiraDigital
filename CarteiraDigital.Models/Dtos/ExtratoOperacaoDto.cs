using CarteiraDigital.Models.Extensions;

namespace CarteiraDigital.Models.Dtos
{
    public struct ExtratoOperacaoDto
    {
        public string Data { get; set; }
        public string TipoOperacao { get; set; }
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public string TipoMovimentacao { get; set; }
        public string SaldoAnterior { get; set; }
        public string Erro { get; set; }

        public static explicit operator ExtratoOperacaoDto(OperacaoDto operacaoDto)
        {
            return new ExtratoOperacaoDto
            {
                Data = operacaoDto.Data.ParaData(),
                TipoOperacao = operacaoDto.TipoOperacao.ObterDescricao(),
                Valor = operacaoDto.Valor.ParaMoeda(),
                Descricao = operacaoDto.Descricao,
                Status = operacaoDto.Status.ObterDescricao(),
                TipoMovimentacao = operacaoDto.TipoMovimentacao.ObterDescricao(),
                SaldoAnterior = operacaoDto.SaldoAnterior.ParaMoeda(),
                Erro = operacaoDto.Erro
            };
        }
    }
}
