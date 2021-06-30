
namespace Model
{
    public class EquipamentoModel
    {
        public const string TIPO_CONDICIONADOR = "CONDICIONADOR";
        public const string TIPO_LUZES = "LUZES";

        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Descricao { get; set; }
        public int Sala { get; set; }
        public string TipoEquipamento { get; set; }
        public int? HardwareDeSala { get; set; }
    }
}
