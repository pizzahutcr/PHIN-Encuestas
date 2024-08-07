using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveys.Models
{
    public class PreguntasDet
    {
        public int IdPregunta { get; set; }
        public int IdEncuesta { get; set; }
        public string descripcion { get; set; }
        public int idTipoResp { get; set; }
        public DateTime fechaReg { get; set; }      
        public int numPregunta { get; set; }
        public bool activo { get; set; }
        public bool obligatorio { get; set; }
        public string listaCodRest { get; set; }
        public string descCorta { get; set; }
        public OpcEnc opcEncs { get; set; }
    }
}