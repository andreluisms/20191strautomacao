﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalasUfsWeb.Models.ViewModels
{
    public class PlanejamentoViewModel
    {
        public int Id { get; set; }
        public string Periodo { get; set; }
        public string Horario { get; set; }
        public string DiaSemana { get; set; }
        public string Objetivo { get; set; }
        public SalaModel SalaId { get; set; }
        public UsuarioModel UsuarioId { get; set; }
    }
}
