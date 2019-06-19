using System.Data;

namespace ParamedicMedicosPrestaciones.Models
{
    public class Periodo
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }

        public Periodo()
        { }

        public Periodo(DataRow dr)
        {
            this.ID = dr["Periodo"].ToString();
            this.Descripcion = dr["PeriodoStr"].ToString();
        }
    }
}