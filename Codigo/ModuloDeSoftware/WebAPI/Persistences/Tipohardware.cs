using System;
using System.Collections.Generic;

namespace Persistence
{
    public partial class Tipohardware
    {
        public Tipohardware()
        {
            Hardwaredesala = new HashSet<Hardwaredesala>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }

        public ICollection<Hardwaredesala> Hardwaredesala { get; set; }
    }
}
