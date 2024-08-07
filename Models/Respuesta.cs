using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveys.Models
{
    public class Respuesta
    {
        public int Id { get; set; }
        public int PreguntaId { get; set; }
        public string TextoRespuesta { get; set; }
    }
}