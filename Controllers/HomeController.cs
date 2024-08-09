﻿using Antlr.Runtime.Misc;
using Surveys.Models;
using Surveys.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using UAParser;
//using System.Drawing.Printing;
//using System.Data.SqlClient;

namespace Surveys.Controllers
{

    public class HomeController : Controller
    {
        RepoEncuesta repoEnc = new RepoEncuesta();
        RepoPreguntasEnc repoPregEnc = new RepoPreguntasEnc();
        RepoPreguntasDet repoPregDet = new RepoPreguntasDet();
        RepoOpcEnc repoOpcEnc = new RepoOpcEnc();
        RepoOpcDet repoOpcDet = new RepoOpcDet();
        RepoRespuestaEnc repoRespEnc = new RepoRespuestaEnc();
        RepoRespuestaDet repoRespDet = new RepoRespuestaDet();
        string pathLogs = Properties.Settings.Default.PathLog;//para los logs

 
        public ActionResult Index(string ide)//, int pagina = 1//pag
        {
            int id = 1;//encuesta
            string codRest = string.Empty;//restaurante
           /* string tokenIde = string.Empty; //pag
            string parametro = string.Empty;*/
           
            //bool existe = false;
            try
            {
                //**prueba
                // Generar un ID aleatorio usando el método de utilidad
                //string randomId = Utils.GenerateRandomId();
                // Aquí puedes hacer lo que necesites con el ID generado
                // ViewData["PageId"] = randomId;

                //pruebas para user agent
                /* string userAgent = Request.UserAgent;
                 var uaParser = Parser.GetDefault();
                 var clientInfo = uaParser.Parse(userAgent);
                 ViewBag.Browser = clientInfo.UA.Family;
                 ViewBag.OS = clientInfo.OS.Family;
                 ViewBag.Device = clientInfo.Device.Family;
                 ViewBag.IsMobile = clientInfo.Device.IsMobile();
                 ViewBag.DeviceModel = clientInfo.Device.Model;*/

                 if (!string.IsNullOrWhiteSpace(ide)) // Verifica si la cadena no es nula, vacía o contiene solo espacios en blanco
                 {
                     ide = ide.Trim();                    
                     if (ide.Contains("_"))//trae codRest
                     {
                         var temp = ide.Split('_');
                         //calificacion = Convert.ToInt32(temp[1]);
                         codRest = temp[0];
                         string valor = temp[1];
                       // tokenIde = valor;//pag
                         if (string.IsNullOrEmpty(codRest) || codRest == "")
                         {
                             ViewBag.BackgroundImageUrl = "/assets/img/default.jpeg";
                             ViewBag.ApplyDisplay = "flex"; // Indicar si se debe aplicar q tipo de display
                             return View();//que mostrar? si no coinciden el ide
                         }
                         id = repoEnc.ExisteEncuesta(true, codRest, valor);//obtenemos el id de la encuesta
                         if (id == 0)
                         {
                             ViewBag.BackgroundImageUrl = "/assets/img/default.jpeg";
                             ViewBag.ApplyDisplay = "flex"; // Indicar si se debe aplicar q tipo de display
                             return View();//que mostrar? si no coinciden el ide
                         }          
                         //else
                         //{
                         //    switch (codRest)
                         //    {
                         //        case "40":
                         //            TempData["DesStore"] = "Restaurante Rohrmoser";
                         //            break;
                         //        case "45":
                         //            TempData["DesStore"] = "Restaurante Río Oro";
                         //            break;
                         //        case "36":
                         //            TempData["DesStore"] = "Restaurante Cartago";
                         //            break;
                         //        case "02":
                         //            TempData["DesStore"] = "Restaurante Guadalupe";
                         //            break;
                         //        default:
                         //            TempData["DesStore"] = "Restaurante Pizza Hut";
                         //            break;
                         //    }
                         //}
                     }
                     else
                     {
                         //**Encuesta sin codRest **                                            
                         id = repoEnc.ExisteEncuesta(true, string.Empty, ide);//obtenemos el id de la encuesta
                         codRest = string.Empty;
                         if (id == 0)
                         {
                             ViewBag.BackgroundImageUrl = "/assets/img/default.jpeg";
                             ViewBag.ApplyDisplay = "flex"; // Indicar si se debe aplicar q tipo de display
                             return View();//que mostrar? si no coinciden el ide
                         }

                     }
                 }
                 else
                 {
                     ViewBag.ApplyDisplay = "flex"; // Indicar si se debe aplicar q tipo de display
                     ViewBag.BackgroundImageUrl = "/assets/img/default.jpeg";
                     return View();//que mostrar? si no coinciden el ide
                 }

                 //** continua obteniendo las preguntas **
                 Encuesta enc = new Encuesta();
                 enc = repoEnc.GetEncuesta(true, id, codRest);
                 enc.codRest = codRest;//asignamos el q tiene q recibir del link....
                /*enc.token = tokenIde;//obtenemos el token/identificador q trae el link
                enc.parametro= ide;*///pag
                 TempData["DesStore"] = enc.nombreLocal; //nombre del local
                 TempData["Sub"] = enc.subtitulo;
                 if (enc.urlFondo == "n/a")//sino trae fondo se le asigna el default
                 {
                     ViewBag.ApplyDisplay = "contents"; // Indicar si se debe aplicar q tipo de display
                     ViewBag.BackgroundImageUrl = "/assets/img/fondo4.jpeg";
                 }
                 else
                 {
                     ViewBag.ApplyDisplay = "contents"; // Indicar si se debe aplicar q tipo de display
                     ViewBag.BackgroundImageUrl = enc.urlFondo;//asignar el fondo q tiene en a bd
                 }


                 //*****PREGUNTAS-ENCABEZADO***               
                 enc.ListPreguntasEncts = repoPregEnc.GetListAll(true, id,codRest);
                 foreach (var tipoenc in enc.ListPreguntasEncts)
                 {
                     tipoenc.opcEncs = repoOpcEnc.GetOpcEnc(true, tipoenc.idTipoResp); //obtenemos el tipo de pregunta
                     if (tipoenc.opcEncs.activaCodRest)//buscamos la lista de respuestas
                     {
                         tipoenc.opcEncs.listaOpcDet = repoOpcDet.GetListAllOpcDet(true, tipoenc.idTipoResp, codRest);
                     }
                     else
                     {
                         tipoenc.opcEncs.listaOpcDet = repoOpcDet.GetListAllOpcDet(true, tipoenc.idTipoResp, "0");
                     }

                 }
                 //*****PREGUNTAS-DETALLADAS***
                 enc.listPreguntasDets = repoPregDet.GetListAll(true, id, codRest);
                 foreach (var tipoenc in enc.listPreguntasDets)
                 {
                     tipoenc.opcEncs = repoOpcEnc.GetOpcEnc(true, tipoenc.idTipoResp); //obtenemos el tipo de pregunta
                     if (tipoenc.opcEncs.activaCodRest)//buscamos la lista de respuestas
                     {
                         tipoenc.opcEncs.listaOpcDet = repoOpcDet.GetListAllOpcDet(true, tipoenc.idTipoResp, codRest);
                     }
                     else
                     {
                         tipoenc.opcEncs.listaOpcDet = repoOpcDet.GetListAllOpcDet(true, tipoenc.idTipoResp, "0");
                     }
                 }

            //*prueba de pag
            //Paginación: Mostrar máximo 5 preguntas por página
              /*enc.TotalPreguntas = enc.listPreguntasDets.Count;
              enc.PreguntasPorPagina = 5;
              enc.TotalPaginas = (int)Math.Ceiling(enc.TotalPreguntas / (double)enc.PreguntasPorPagina);               
                enc.PaginaActual = pagina;
                enc.ListPreguntasEncts = enc.ListPreguntasEncts.Skip((pagina - 1) * enc.PreguntasPorPagina).Take(enc.PreguntasPorPagina).ToList();
                enc.listPreguntasDets = enc.listPreguntasDets.Skip((pagina - 1) * enc.PreguntasPorPagina).Take(enc.PreguntasPorPagina).ToList();
                */
               // var questionsForPage = enc.listPreguntasDets.Skip((pagina - 1) * enc.PreguntasPorPagina).Take(enc.PreguntasPorPagina).ToList();
                //*fin


                return View(enc);
                
            }
            catch (Exception ex)
            {
                //log
                DateTime hoy = DateTime.Now;
                using (StreamWriter writer = new StreamWriter(pathLogs, true))
                { writer.WriteLine("**Ocurrió un error Index() " + id + " fecha:" + hoy + " ex:" + ex.ToString()); }
                //fin 
            }

            return View();
        }

   
        // Acción para guardar las respuestas
        [HttpPost]
        public ActionResult GuardarRespuestas(FormCollection form,string txtIdEncuesta, string txtCodRest, string txtUrl)
        {
            // Crear una lista para almacenar las respuestas
            //List<RespuestaEnc> respuestasEnc = new List<RespuestaEnc>();
           // List<RespuestaDet> respuestasDet = new List<RespuestaDet>();
            int idEncuesta = Convert.ToInt32(txtIdEncuesta);
            string codRest = txtCodRest;//"40";//restaurante

            //**userAgent

            //string ipAddress = Request.UserHostAddress;
            //ViewBag.IpAddress = ipAddress;

            string userAgent = Request.UserAgent;
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);
            string uaBrowser = clientInfo.UA.Family;
            string uaOS= clientInfo.OS.Family;
            string uaDevice= clientInfo.Device.Family;
            string uaDeviceModel = clientInfo.Device.Model;
            if (string.IsNullOrWhiteSpace(uaDeviceModel))
            {
                uaDeviceModel = "N/A";
            }

            //fin

            int num = repoRespEnc.GetNumEncuesta(idEncuesta);//obtener el max de # de encuesta, la cant de clientes q lleva
           // int numDet = repoRespDet.GetNumEncuesta(idEncuesta, codRest);//obtener el max de # de encuesta, la cant de clientes q lleva

            // Recorrer cada clave en el formulario
            foreach (string key in form.AllKeys)
            {
                if(key == "IdEncuesta")
                {
                    idEncuesta = Convert.ToInt32(form[key]);
                }
                else
                {
                    // Parsear el Id de la pregunta desde el nombre del campo
                    if (key.StartsWith("pregEnc_txt[") || key.StartsWith("pregEnc_chk[") || key.StartsWith("pregEnc_rad[") || key.StartsWith("pregEnc_dat[") || key.StartsWith("pregEnc_tar[") || key.StartsWith("pregEnc_tar[") || key.StartsWith("pregEnc_lts["))
                    {
                        int preguntaId;
                        if (int.TryParse(key.Substring(12, key.IndexOf("]") - 12), out preguntaId))
                        {
                            // Obtener el valor enviado
                            string valor = form[key];
                            if (!key.StartsWith("pregEnc_dat["))//ya que la fecha llega 2024-07-24
                            {
                                if (valor.Contains("-"))
                                {
                                    if (valor.Contains(","))//trae los valores de los checkbox ejem: false,Malo-0,false,false,false
                                    {
                                        var valores = valor.Split(',');
                                        valor = "";
                                        foreach (var valorValores in valores)
                                        {
                                            if (valorValores.Contains("-"))
                                            {
                                                var temp0 = valorValores.Split('-');
                                                //calificacion += Convert.ToInt32(temp0[1]);
                                                valor += temp0[0] + " ";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var temp = valor.Split('-'); //radio, dropDownList
                                       // calificacion = Convert.ToInt32(temp[1]);
                                        valor = temp[0];
                                    }

                                }
                            }
                            // Crear una nueva respuesta y agregarla a la lista

                            string[] pregunta = repoPregEnc.GetPregEncDesc(true, idEncuesta, preguntaId);//obtener la descripción "pregunta" y descripción corta contatenadas
                            //var temp2 = pregunta.Split('-');// temp[0]=pregunta,  temp[1]=desccorta                                                                      
                            //guardar los datos de la persona q responde la encuesta
                            UserAgentEnc user = new UserAgentEnc
                            { 
                                UABrowser =uaBrowser,
                                UAOS = uaOS,
                                UADevice = uaDevice,
                                UADeviceModel = uaDeviceModel
                            };
                            RespuestaEnc nuevaRespuesta = new RespuestaEnc
                            {
                                idEncuesta = idEncuesta,
                                numEncuesta = num,
                                idPregEnc = preguntaId,
                                codRest = codRest,
                                descPregEnc = pregunta[0],
                                respuesta = valor,
                                userAgent = user,
                                descCorta = pregunta[1]
                            };
                           // respuestasEnc.Add(nuevaRespuesta);
                            // Guardar nuevaRespuesta en la base de datos
                            repoRespEnc.NewRespEnc(nuevaRespuesta);
                        }
                    }
                    else
                    {
                        //para el detalle
                        if (key.StartsWith("pregDet_txt[") || key.StartsWith("pregDet_chk[") || key.StartsWith("pregDet_rad[") || key.StartsWith("pregDet_dat[") || key.StartsWith("pregDet_tar[") || key.StartsWith("pregDet_lts["))
                        {
                            int preguntaId;
                            if (int.TryParse(key.Substring(12, key.IndexOf("]") - 12), out preguntaId))
                            {
                                // Obtener el valor enviado
                                string valor = form[key];
                                int calificacion = 0;
                               
                                if (!key.StartsWith("pregDet_dat["))//ya que la fecha llega 2024-07-24
                                {
                                    if (valor.Contains("-"))
                                    {
                                        if (valor.Contains(","))//trae los valores de los checkbox ejem: false,Malo-0,false,false,false
                                        {
                                            var valores = valor.Split(',');
                                            valor = "";
                                            foreach (var valorValores in valores)
                                            {
                                                if (valorValores.Contains("-"))
                                                {
                                                    var temp0 = valorValores.Split('-');
                                                    calificacion += Convert.ToInt32(temp0[1]);
                                                    valor += temp0[0]+" ";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var temp = valor.Split('-');//radio, dropDownList
                                            calificacion = Convert.ToInt32(temp[1]);
                                            valor = temp[0];
                                        }
                                    
                                    }
                                }
                                // Crear una nueva respuesta y agregarla a la lista
                                string[] pregunta = repoPregDet.GetPregDetDesc(true, idEncuesta, preguntaId);//obtener la descripción "pregunta"
                                //int calificacion = repoOpcDet.GetCalificacion(true, preguntaId);//obtiene la calificación de esa respuesta
                                RespuestaDet nuevaRespuesta = new RespuestaDet
                                {
                                    idEncuesta = idEncuesta,
                                    numEncuesta = num,
                                    idPregDet = preguntaId,
                                    codRest = codRest,
                                    descPregDet = pregunta[0],
                                    respuesta = valor.Trim(),
                                    calificacion = calificacion,
                                    descCorta = pregunta[1]
                                };
                                //respuestasDet.Add(nuevaRespuesta);
                                // Guardar nuevaRespuesta en la base de datos
                                repoRespDet.NewRespDet(nuevaRespuesta);
                            }
                        }
                    }
                }
            }
            // Almacena los parámetros en TempData
            TempData["Url"] = txtUrl;
            Encuesta enc = new Encuesta();
            enc = repoEnc.GetEncuesta(true, idEncuesta, codRest);
            TempData["msj"] = enc.mensaCierre;
            // Aquí puedes guardar las respuestas en la base de datos o realizar otras operaciones
            //return RedirectToAction("Completada", new { Url= txtUrl });         
            return RedirectToAction("Completada");
        }
        public ActionResult Completada()//Encuesta completada
        {
            //ViewBag.Message = TempData["msj"]; //"Gracias por completar la encuesta";
            // Recupera los parámetros de TempData
            string Url = TempData["Url"] as string;
            //int param2 = TempData.ContainsKey("Param2") ? (int)TempData["Param2"] : 0;

            if (Url == "")//sino trae fondo se le asigna el default
            {
                ViewBag.BackgroundImageUrl = "/assets/img/fondo4.jpeg";
            }
            else
            {
                ViewBag.BackgroundImageUrl = Url;//asignar el fondo q tiene en a bd
            }
            var mensaje = TempData["msj"] as string;//para separar el mensaje de cierre
            var temp = mensaje.Split('.');
            ViewBag.Message = temp;


            //List<string> myList = new List<string> { "element1", "element2",
            //                             "element3", "element4",
            //                             "element5",
            //                           };
            //ViewBag.MyVariable = myList.ToArray();
            return View();
        }

        //**prueba de paginación

        [HttpPost]
        public ActionResult SaveResponses(FormCollection form)//, int page
        {
            // Guardar las respuestas actuales en sesión para mantener el estado entre páginas
            var sessionKey = $"Survey_{1}";
            var responses = Session[sessionKey] as Dictionary<int, string> ?? new Dictionary<int, string>();

            //foreach (var response in model.ListPreguntasEncts)
            //{
            //    responses[response.IdPregEnc] = response.descripcion;
            //}
            //foreach (var response in model.listPreguntasDets)
            //{
            //    responses[response.IdPregunta] = response.descripcion;
            //}
            //Session[sessionKey] = responses;
            //*

         
                // Crear una lista para almacenar las respuestas
                //List<RespuestaEnc> respuestasEnc = new List<RespuestaEnc>();
                // List<RespuestaDet> respuestasDet = new List<RespuestaDet>();
                int idEncuesta = Convert.ToInt32(1);
          
            // Recorrer cada clave en el formulario
            foreach (string key in form.AllKeys)
            {
                if (key == "IdEncuesta")
                {
                    idEncuesta = Convert.ToInt32(form[key]);
                }
                else
                {
                    // Parsear el Id de la pregunta desde el nombre del campo
                    if (key.StartsWith("pregEnc_txt[") || key.StartsWith("pregEnc_chk[") || key.StartsWith("pregEnc_rad[") || key.StartsWith("pregEnc_dat[") || key.StartsWith("pregEnc_tar[") || key.StartsWith("pregEnc_tar[") || key.StartsWith("pregEnc_lts["))
                    {
                        int preguntaId;
                        if (int.TryParse(key.Substring(12, key.IndexOf("]") - 12), out preguntaId))
                        {
                            // Obtener el valor enviado
                            string valor = form[key];
                            //guardar en la sesion
                            responses[preguntaId] = valor;//preguntaID con la respuesta
                        }
                    }
                    else
                    {
                        //para el detalle
                        if (key.StartsWith("pregDet_txt[") || key.StartsWith("pregDet_chk[") || key.StartsWith("pregDet_rad[") || key.StartsWith("pregDet_dat[") || key.StartsWith("pregDet_tar[") || key.StartsWith("pregDet_lts["))
                        {
                            int preguntaId;
                            if (int.TryParse(key.Substring(12, key.IndexOf("]") - 12), out preguntaId))
                            {
                                // Obtener el valor enviado
                                string valor = form[key];
                                //guardar en la sesion
                                responses[preguntaId] = valor;//preguntaID con la respuesta

                            }
                        }
                    }
                }

            }
            Session[sessionKey] = responses;
            return Json(new { success = true });
        }


        [HttpPost]
        public ActionResult SubmitSurvey(int IdEncuesta)
        {
            var sessionKey = $"Survey_{IdEncuesta}";
            var responses = Session[sessionKey] as Dictionary<int, string>;

            if (responses != null)
            {
                SaveResponsesToDatabase(IdEncuesta, responses);
                Session.Remove(sessionKey);
            }

            return RedirectToAction("ThankYou");
        }
        private void SaveResponsesToDatabase(int surveyId, Dictionary<int, string> responses)
        {
         
            foreach (var response in responses)
            {
                var algo = response.Key + " respuesta: " + response.Value;                
            }
            
        }


        //fin

    }
}