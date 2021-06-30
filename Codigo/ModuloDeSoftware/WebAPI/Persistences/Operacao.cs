using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Operacao
    {
        public Operacao()
        {
            Codigoinfravermelho = new HashSet<Codigoinfravermelho>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public ICollection<Codigoinfravermelho> Codigoinfravermelho { get; set; }
    }
}
