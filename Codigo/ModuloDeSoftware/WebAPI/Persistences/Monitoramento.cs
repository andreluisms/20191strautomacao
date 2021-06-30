

namespace Persistence
{
    public partial class Monitoramento
    {
        public int Id { get; set; }
        public byte Estado { get; set; }
        public int Equipamento { get; set; }

        public Equipamento EquipamentoNavigation { get; set; }
    }
}
