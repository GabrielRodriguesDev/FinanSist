using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestArchitectureReviewOne.Domain.Commands.Usuario;
using TestArchitectureReviewOne.Domain.Entities;
using TestArchitectureReviewOne.Domain.Helpers;
using TestArchitectureReviewOne.Domain.Interfaces.Infrastructure;
using TestArchitectureReviewOne.Domain.Interfaces.Repositories;
using TestArchitectureReviewOne.Domain.Interfaces.Services;

namespace TestArchitectureReviewOne.Domain.Services
{
    public class UsuarioService : IUsuarioService
    {
        #region  Property
        private IUsuarioRepository _usuarioRepository;

        private IUnitOfWork _uow;

        #endregion

        #region  Constructor
        public UsuarioService(IUsuarioRepository usuarioRepository, IUnitOfWork uow)
        {
            _usuarioRepository = usuarioRepository;
            _uow = uow;
        }
        #endregion

        #region  Method
        public GenericCommandResult Create(CreateUsuarioCommand createCommand)
        {
            createCommand.Validate();
            if (createCommand.Invalid)
                return new GenericCommandResult(false, "Ops, algo deu errado!", createCommand.Notifications);

            var usuariodb = _usuarioRepository.ExistePorEmail(createCommand.Email);
            if (usuariodb)
                return new GenericCommandResult(false, "Desculpe, e-mail já cadastrado em outro usuário.");

            createCommand.SenhaTemporaria = CriptoHelper.createRandomPassword();

            var usuario = new Usuario(createCommand);

            _uow.BeginTransaction();

            try
            {
                _usuarioRepository.Create(usuario);
                _uow.Commit();

            }
            catch (System.Exception)
            {

                _uow.Rollback();
                throw;
            }

            return new GenericCommandResult(true, $"Usuário {usuario.Nome} salvo com sucesso", new
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                SenhaTemporaria = $"Senha temporária do usuário {usuario.Nome}: {createCommand.SenhaTemporaria}"
            });

        }

        public GenericCommandResult Delete(Guid id)
        {
            var usuariodb = _usuarioRepository.ExistePorId(id);
            if (!usuariodb)
                return new GenericCommandResult(false, "Desculpe, usuário não foi localizado.");

            _uow.BeginTransaction();

            try
            {
                _usuarioRepository.Detete(id);
                _uow.Commit();
            }
            catch (System.Exception)
            {
                _uow.Rollback();
                throw;
            }
            return new GenericCommandResult(true, "Usuário removido com sucesso!", null);
        }

        public GenericCommandResult Update(UpdateUsuarioCommand updateCommand)
        {
            updateCommand.Validate();
            if (updateCommand.Invalid)
                return new GenericCommandResult(false, "Ops, algo deu errado!", updateCommand.Notifications);

            var usuario = _usuarioRepository.Get(updateCommand!.Id!.Value);
            if (usuario == null)
                return new GenericCommandResult(false, "Usuário não existe.");
            if (usuario.Email != updateCommand.Email)
            {
                var usuariodb = _usuarioRepository.ExistePorEmail(updateCommand.Email);
                if (usuariodb)
                    return new GenericCommandResult(false, "Desculpe, e-mail já cadastrado em outro usuário.");
            }
            usuario.Update(updateCommand);

            _uow.BeginTransaction();

            try
            {
                _usuarioRepository.Update(usuario);
                _uow.Commit();
            }
            catch (System.Exception)
            {
                _uow.Rollback();
                throw;
            }
            return new GenericCommandResult(true, "Usuário atualizado com sucesso!", new
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            });
        }
        #endregion

    }
}