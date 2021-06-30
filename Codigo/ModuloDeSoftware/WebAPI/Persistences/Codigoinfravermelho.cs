using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Codigoinfravermelho
    {
        public int Id { get; set; }
        public int Equipamento { get; set; }
        public int Operacao { get; set; }
        public string Codigo { get; set; }

        public Equipamento EquipamentoNavigation { get; set; }
        public Operacao OperacaoNavigation { get; set; }
    }
}
