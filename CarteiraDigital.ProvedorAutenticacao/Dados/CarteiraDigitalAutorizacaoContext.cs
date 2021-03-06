﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarteiraDigital.ProvedorAutenticacao.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CarteiraDigital.ProvedorAutenticacao.Dados
{
    public class CarteiraDigitalAutorizacaoContext : IdentityDbContext<Usuario>
    {
        private static bool tabelasCriadas;

        public CarteiraDigitalAutorizacaoContext(DbContextOptions<CarteiraDigitalAutorizacaoContext> options) : base(options)
        {
            Database.EnsureCreated();

            if (!tabelasCriadas)
            {
                var creator = (RelationalDatabaseCreator)this.Database.GetService<IDatabaseCreator>();

                try
                {
                    creator.CreateTables();
                }
                catch
                {
                }

                tabelasCriadas = true;
            }
        }
    }
}
