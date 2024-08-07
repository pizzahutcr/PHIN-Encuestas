﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveys.Models
{
    public class Encuesta
    {
        public int IdEncuesta { get; set; }
        public string descripcion { get; set;}
        public DateTime fechaIni { get; set; }
        public DateTime fechaFin { get; set; }
        public bool activo { get; set; }
        public string responsable { get; set; }
        public string departamento { get; set; }
        public string listaCodRest { get; set; }
        public string token { get; set; }
        public List<PreguntasDet> listPreguntasDets { get; set; }
        public List<PreguntasEnc> ListPreguntasEncts { get; set; }
        public string codRest { get; set; }
        //*prueba de pag
        public int PreguntasPorPagina { get; set; } = 2;
        public int PaginaActual { get; set; } = 1;
        public int TotalPreguntas { get; set; }
        public int TotalPaginas { get; set; }
        //*fin
        public string urlFondo { get; set; } = "n/a";
        public string nombreLocal { get; set; } = string.Empty;
        public string subtitulo { get; set; }
        public string mensaCierre { get; set; }
        public string parametro { get; set; }= string.Empty;

        public Encuesta()
        {
 
        }
        public Encuesta(int idEncuesta, string descripcion, DateTime fechaIni, DateTime fechaFin, bool activo, string responsable, string departamento, string listaCodRest, string token, List<PreguntasDet> listPreguntasDets, List<PreguntasEnc> listPreguntasEncts)
        {
            IdEncuesta = idEncuesta;
            this.descripcion = descripcion;
            this.fechaIni = fechaIni;
            this.fechaFin = fechaFin;
            this.activo = activo;
            this.responsable = responsable;
            this.departamento = departamento;
            this.listaCodRest = listaCodRest;
            this.token = token;
            this.listPreguntasDets = listPreguntasDets;
            ListPreguntasEncts = listPreguntasEncts;
        }
    }
}