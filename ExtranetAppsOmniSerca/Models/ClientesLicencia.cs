using System;

namespace ExtranetAppsOmniSerca.Models
{
    public class ClientesLicencia
    {
        public int ID { get; set; }

        public int LicenciaID { get; set; }

        public long ClienteID { get; set; }

        //[Required]
        //[Display(Name = "Catalog")]
        public string CnnCatalog { get; set; }

        //[Required]
        //[Display(Name = "Usuario")]
        public string CnnUser { get; set; }

        //[Required]
        //[Display(Name = "Password")]
        public string CnnPassword { get; set; }

        //[DataType(DataType.Date)]
        //[Column(TypeName = "DateTime2")]
        //[Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaDeVencimiento { get; set; }

        //[Display(Name = "Servidor")]
        public string ConexionServidor { get; set; }

    }
}