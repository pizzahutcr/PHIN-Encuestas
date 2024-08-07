using QRCoder;
using Surveys.Models;
using Surveys.Repos;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveys.Controllers
{
    public class QRController : Controller
    {
        RepoQR repoQr = new RepoQR();
        // GET: QR
        public ActionResult Index()
        {
            //return View(new QR());//v1
            //List<SelectListItem> lst = repoQr.GetOptionsLocales();
            //ViewData["misTiendas"] = lst;
            //v1
            // List<SelectListItem> lst2 = repoQr.GetOptionsEncuestas();
            // ViewData["misEncuestas"] = lst2;
            //v2           
            // ViewBag.Enc = new SelectList(repoQr.GetListEncuestas(), "idEncuesta", "nombre" );
            //V3
            var model = new QRViewModel
            {
                ltsEncuestas = new SelectList(repoQr.GetListEncuestas(), "idEncuesta", "nombre"),
                ltsLocales = Enumerable.Empty<SelectListItem>()
        };
            return View(model);
            // return View();
        }

        [HttpGet]
        public JsonResult GetLocales(string token)//cargamos los locales de dicha encuesta
        {
            if (token == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            string lista = repoQr.GetListCodRest(true, token);//obtengo la lista de codRest de la encuesta
            var citiesList = repoQr.GetListLocales(lista);          
            return Json(citiesList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(QRViewModel model) //v2 y v1: string txtCodRest, string txtID,
        {
            //v1
            /* List<SelectListItem> lst = repoQr.GetOptionsLocales("");//ver***
             ViewData["misTiendas"] = lst;
             List<SelectListItem> lst2 = repoQr.GetOptionsEncuestas();
             ViewData["misEncuestas"] = lst2;*/
            //v2
            //ViewBag.Enc = new SelectList(repoQr.GetListEncuestas(), "idEncuesta", "nombre");
            //V3

            string txtCodRest = model.SelectedCodRest;
            string txtID = model.SelectedEncuestaId;
            model.ltsEncuestas = new SelectList(repoQr.GetListEncuestas(), "idEncuesta", "nombre");
            string lista = repoQr.GetListCodRest(true, txtID);//obtengo la lista de codRest de la encuesta
            if (!string.IsNullOrEmpty(model.SelectedCodRest))
            {
                model.ltsLocales = new SelectList(repoQr.GetListLocales(lista), "codRest", "nombre");
            }
            else
            {
                model.ltsLocales = Enumerable.Empty<SelectListItem>();
                
            }

            //fin v3
            string url = "";
            if (string.IsNullOrEmpty(txtID))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Parameters cannot be null or empty.");
            }
            if (!string.IsNullOrEmpty(txtCodRest) && !string.IsNullOrEmpty(txtID))
            {
               url = $"https://encuestas.pizzahutcr.com?ide={txtCodRest}_{txtID}";
            }
            else
            {
                url = $"https://encuestas.pizzahutcr.com?ide={txtID}";
            }

            
            //string url = $"http://192.168.9.204:8091?ide={parameters.codRest}_{parameters.idEncuesta}";
            //string url = $"http://172.16.20.11:9099?ide={parameters.codRest}_{parameters.idEncuesta}";
            //ViewBag.Url = url;
            //parameters.GeneratedUrl = url;
            ViewBag.QrCodeImage = GenerateQrCodeImage(url);
            ViewBag.UrlStr = url;
            return View(model);
        }

        //[HttpPost]  //v1 con input text de parámetros
        //public ActionResult Index(QR parameters)
        //{
        //    if (parameters == null || string.IsNullOrEmpty(parameters.idEncuesta))
        //    {
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Parameters cannot be null or empty.");
        //    }

        //    string url = $"https://encuestas.pizzahutcr.com?ide={parameters.codRest}_{parameters.idEncuesta}";          
        //    //string url = $"http://192.168.9.204:8091?ide={parameters.codRest}_{parameters.idEncuesta}";
        //    //string url = $"http://172.16.20.11:9099?ide={parameters.codRest}_{parameters.idEncuesta}";
        //    //192.168.9.204
        //    //ViewBag.Url = url;
        //    parameters.GeneratedUrl = url;
        //    ViewBag.QrCodeImage = GenerateQrCodeImage(url);
        //    return View(parameters);
        //}
        private string GenerateQrCodeImage(string url, int size = 20)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrCodeData))
                {
                    using (var bitmap = qrCode.GetGraphic(size))
                    {
                        using (var stream = new MemoryStream())
                        {
                            bitmap.Save(stream, ImageFormat.Png);
                            return "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
                        }
                    }
                }
            }
        }

        /*
         //generar qr
         
            if (string.IsNullOrEmpty(url))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "URL cannot be null or empty.");
            }

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrCodeData))
                {
                    using (var bitmap = qrCode.GetGraphic(20))
                    {
                        using (var stream = new MemoryStream())
                        {
                            bitmap.Save(stream, ImageFormat.Png);
                            return File(stream.ToArray(), "image/png");
                        }
                    }
                }
            }
*/

        /*
                 [HttpGet]
        public ActionResult ShowQr(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "URL cannot be null or empty.");
            }

            ViewBag.Url = url;
            return View();
        }

       
        [HttpGet]
        public ActionResult GenerarQr(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "URL cannot be null or empty.");
            }

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrCodeData))
                {
                    using (var bitmap = qrCode.GetGraphic(20))
                    {
                        using (var stream = new MemoryStream())
                        {
                            bitmap.Save(stream, ImageFormat.Png);
                            return File(stream.ToArray(), "image/png");
                        }
                    }
                }
            }
        }
        
         
         */

        /*
         //guardar en un archivo

        public void GenerateMultipleQrCodes(string[] urls)
    {
        Parallel.ForEach(urls, url =>
        {
            string filePath = Path.Combine("GeneratedQrCodes", $"{Guid.NewGuid()}.png");
            GenerateQrCode(url, filePath);
        });
    }

    private void GenerateQrCode(string url, string filePath, int size = 20)
    {
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using (var qrCode = new QRCode(qrCodeData))
            {
                using (var bitmap = qrCode.GetGraphic(size))
                {
                    bitmap.Save(filePath, ImageFormat.Png);
                }
            }
        }
    }

         */
    }
}