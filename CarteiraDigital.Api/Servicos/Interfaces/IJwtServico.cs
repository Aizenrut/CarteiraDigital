﻿namespace CarteiraDigital.Api.Servicos
{
    public interface IJwtServico
    {
        string ObterSubject(string token);
        string FormatarToken(string token);
    }
}
