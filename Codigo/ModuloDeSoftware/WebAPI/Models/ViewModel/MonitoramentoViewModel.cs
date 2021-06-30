using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModel
{
    public class MonitoramentoViewModel
    {
        public int Id { get; set; }
        public bool Luzes { get; set; }
        public bool ArCondicionado { get; set; }
        public int SalaId { get; set; }
        public bool SalaParticular { get; set; }
    }
}
