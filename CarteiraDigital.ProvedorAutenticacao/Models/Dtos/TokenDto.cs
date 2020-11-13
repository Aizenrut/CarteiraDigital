using System;

namespace CarteiraDigital.ProvedorAutenticacao.Models
{
    public struct TokenDto
    {
        public string Usuario { get; set; }
        public string Token { get; }
        public DateTime DataEmissao { get; }
        public DateTime DataExpiracao { get; }

        public TokenDto(string usuario, string token, DateTime dataEmissao, DateTime dataExpiracao)
        {
            Usuario = usuario;
            Token = token;
            DataEmissao = dataEmissao;
            DataExpiracao = dataExpiracao;
        }
    }
}
