using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Horariosala
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public string Situacao { get; set; }
        public string Objetivo { get; set; }
        public int Usuario { get; set; }
        public int Sala { get; set; }
        public int? Planejamento { get; set; }

        public Planejamento PlanejamentoNavigation { get; set; }
        public Sala SalaNavigation { get; set; }
        public Usuario UsuarioNavigation { get; set; }
    }
}
