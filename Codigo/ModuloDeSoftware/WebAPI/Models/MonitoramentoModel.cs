using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class MonitoramentoModel
    {
        public int Id { get; set; }
        public bool Estado { get; set; }
        public int EquipamentoId { get; set; }
        public bool SalaParticular { get; set; }

        public EquipamentoModel EquipamentoNavigation { get; set; }
    }
}
