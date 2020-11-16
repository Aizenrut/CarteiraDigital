using CarteiraDigital.Models.Extensions;
using System;

namespace CarteiraDigital.ProvedorAutenticacao.Models
{
    public struct TokenDto
    {
        public string Usuario { get; set; }
        public string Token { get; }
        public string DataEmissao { get; }
        public string DataExpiracao { get; }

        public TokenDto(string usuario, string token, DateTime dataEmissao, DateTime dataExpiracao)
        {
            Usuario = usuario;
            Token = token;
            DataEmissao = dataEmissao.ParaData();
            DataExpiracao = dataExpiracao.ParaData();
        }
    }
}
