﻿using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Models;
using CarteiraDigital.Servicos;
using CarteiraDigital.ProvedorAutenticacao.Builders;
using System;
using CarteiraDigital.Models;

namespace CarteiraDigital.ProvedorAutenticacao.Servicos
{
    public class UsuarioServico : IUsuarioServico
    {
        private readonly UserManager<Usuario> userManager;
        private readonly IContaServico contaServico;
        private readonly IUsuarioBuilder usuarioBuilder;
        private readonly IValidacaoDocumentosServico validadorDocumentos;
        private readonly IConfiguracaoServico configuracaoServico;

        public UsuarioServico(UserManager<Usuario> userManager,
                              IContaServico contaServico,
                              IUsuarioBuilder usuarioBuilder,
                              IValidacaoDocumentosServico validadorDocumentos,
                              IConfiguracaoServico configuracaoServico)
        {
            this.userManager = userManager;
            this.contaServico = contaServico;
            this.usuarioBuilder = usuarioBuilder;
            this.validadorDocumentos = validadorDocumentos;
            this.configuracaoServico = configuracaoServico;
        }

        public async Task<Usuario> ObterPeloNomeAsync(string nomeUsuario)
        {
            return await userManager.FindByNameAsync(nomeUsuario);
        }

        public bool EhUsuarioValido(Usuario usuario)
        {
            return usuario != null;
        }

        public async Task<bool> EhUsuarioValidoAsync(string nomeUsuario)
        {
            return EhUsuarioValido(await ObterPeloNomeAsync(nomeUsuario));
        }

        public async Task<IdentityResult> CadastrarAsync(CadastroUsuarioDto cadastro)
        {
            var usuario = usuarioBuilder.ComNomeUsuario(cadastro.NomeUsuario)
                                        .ComNome(cadastro.Nome)
                                        .ComSobrenome(cadastro.Sobrenome)
                                        .ComCpf(cadastro.Cpf)
                                        .NascidoEm(cadastro.DataNascimento)
                                        .Gerar();

            ValidarUsuario(usuario);
            
            var resultado = await userManager.CreateAsync(usuario, cadastro.Senha);

            if (resultado.Succeeded)
                contaServico.Cadastrar(usuario.UserName);
            
            return resultado;
        }

        public async Task<IdentityResult> AlterarSenhaAsync(Usuario usuario, string senhaAtual, string novaSenha)
        {
            return await userManager.ChangePasswordAsync(usuario, senhaAtual, novaSenha);
        }

        public bool PossuiIdadeMinima(Usuario usuario)
        {
            return (DateTime.Now - usuario.DataNascimento).Days >= configuracaoServico.ObterIdadeMinima() * 365;
        }

        public void ValidarUsuario(Usuario usuario)
        {
            if (!validadorDocumentos.EhCpfValido(usuario.Cpf))
                throw new CarteiraDigitalException("O CPF informado é inválido!");

            if (!PossuiIdadeMinima(usuario))
                throw new CarteiraDigitalException($"Não possui a idade mínima para cadastro ({ configuracaoServico.ObterIdadeMinima() } anos)!");
        }
    }
}
