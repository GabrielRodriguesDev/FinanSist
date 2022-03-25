using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanSist.Domain.Commands.Entidade;

namespace FinanSist.Domain.Entities
{
    public class Entidade : BaseEntity
    {
        #region Property
        public String Nome { get; set; } = null!;
        public String Descricao { get; set; } = null!;
        public bool Ativo { get; set; }
        #endregion

        #region  Constructor
        public Entidade()
        {

        }
        public Entidade(CreateEntidadeCommand cmd)
        {
            this.Nome = cmd.Nome;
            this.Descricao = cmd.Descricao;
            this.Ativo = true;
        }

        public void Update(UpdateEntidadeCommand cmd)
        {
            this.Nome = cmd.Nome;
            this.Descricao = cmd.Descricao;
            this.Ativo = cmd.Ativo;
        }
        #endregion

    }
}