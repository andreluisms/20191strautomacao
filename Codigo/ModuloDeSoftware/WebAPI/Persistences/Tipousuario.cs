using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Tipousuario
    {
        public Tipousuario()
        {
            Usuario = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }

        public ICollection<Usuario> Usuario { get; set; }
    }
}
