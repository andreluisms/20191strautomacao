using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Salaparticular
    {
        public int Id { get; set; }
        public int Usuario { get; set; }
        public int Sala { get; set; }

        public Sala SalaNavigation { get; set; }
        public Usuario UsuarioNavigation { get; set; }
    }
}
