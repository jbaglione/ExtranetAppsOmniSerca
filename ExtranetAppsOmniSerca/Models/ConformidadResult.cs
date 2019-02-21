using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{
    public class ConformidadResult
    {
        public int Resultado { get; set; }
        public string AlertaError { get; set; }
        public ConformidadResult(DataRow dr)
        {
            this.Resultado = Convert.ToInt32(dr["Resultado"].ToString());
            this.AlertaError = dr["AlertaError"].ToString();
        }
    }
}