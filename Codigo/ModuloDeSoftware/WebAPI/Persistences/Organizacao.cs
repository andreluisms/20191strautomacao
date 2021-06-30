using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Organizacao
    {
        public Organizacao()
        {
            Bloco = new HashSet<Bloco>();
            Usuarioorganizacao = new HashSet<Usuarioorganizacao>();
        }

        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }

        public ICollection<Bloco> Bloco { get; set; }
        public ICollection<Usuarioorganizacao> Usuarioorganizacao { get; set; }
    }
}
