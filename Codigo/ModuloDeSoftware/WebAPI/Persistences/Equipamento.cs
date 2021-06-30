using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Equipamento
    {
        public Equipamento()
        {
            Codigoinfravermelho = new HashSet<Codigoinfravermelho>();
            Monitoramento = new HashSet<Monitoramento>();
        }

        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Descricao { get; set; }
        public int Sala { get; set; }
        public string TipoEquipamento { get; set; }
        public int? HardwareDeSala { get; set; }

        public Hardwaredesala HardwareDeSalaNavigation { get; set; }
        public Sala SalaNavigation { get; set; }
        public ICollection<Codigoinfravermelho> Codigoinfravermelho { get; set; }
        public ICollection<Monitoramento> Monitoramento { get; set; }
    }
}
