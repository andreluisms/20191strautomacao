using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Usuarioorganizacao
    {
        public int Id { get; set; }
        public int Organizacao { get; set; }
        public int Usuario { get; set; }

        public Organizacao OrganizacaoNavigation { get; set; }
        public Usuario UsuarioNavigation { get; set; }
    }
}
