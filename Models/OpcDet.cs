using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveys.Models
{
    public class OpcDet
    {
        public int idOpcDet { get; set; }
        public int idTipoResp { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaReg { get; set; }
        public string listaCodRest { get; set; } = string.Empty;
        public bool activo { get; set; }
        public int calificacion { get; set; }

    }
}