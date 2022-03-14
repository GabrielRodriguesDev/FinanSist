using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestArchitectureReviewOne.Domain.Commands.Usuario;
using TestArchitectureReviewOne.Domain.Helpers;

namespace TestArchitectureReviewOne.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        #region Property

        public string Nome { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Telefone { get; private set; } = null!;
        public string? Senha { get; private set; } = null!;
        public bool Ativo { get; private set; }
        public bool ExigirNovaSenha { get; private set; }


        #endregion

        #region Constructor

        public Usuario() { }

        public Usuario(CreateUsuarioCommand cmd)
        {
            this.Nome = cmd.Nome;
            this.Email = cmd.Email;
            this.Telefone = cmd.Telefone;
            this.SenhaPadrao(cmd.SenhaTemporaria!);
            this.Ativo = cmd.Ativo;
            this.ExigirNovaSenha = true;
        }

        public void Update(UpdateUsuarioCommand cmd)
        {
            this.Nome = cmd.Nome;
            this.Email = cmd.Email;
            this.Telefone = cmd.Telefone;
            this.Senha = cmd.Senha;
            this.Ativo = cmd.Ativo;
        }

        #endregion

        #region Method
        public void SenhaPadrao(string senhaTemp)
        {
            this.Senha = CriptoHelper.HashPassword(senhaTemp);
        }
        #endregion
    }
}