using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class ResumenLiquidacionEmpleados
    {
        public List<ResumenLiquidacionEmpleadosItem> Trabajado { get; set; }
        public List<ResumenLiquidacionEmpleadosItem> Esperado { get; set; }
        public List<ResumenLiquidacionEmpleadosItem> Incumplimientos { get; set; }

        public ResumenLiquidacionEmpleados()
        {
            Trabajado = new List<ResumenLiquidacionEmpleadosItem>();
            Esperado = new List<ResumenLiquidacionEmpleadosItem>();
            Incumplimientos = new List<ResumenLiquidacionEmpleadosItem>();
        }
    }
}