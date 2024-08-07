using Antlr.Runtime.Misc;
using Antlr.Runtime.Tree;
using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using UAParser;

namespace Surveys.Repos
{
    public class RepoRespuestaEnc
    {
        string connectionString = Properties.Settings.Default.ConectionString;
        string pathLogs = Properties.Settings.Default.PathLog;//para los logs
        public RepoRespuestaEnc() { }

        public int NewRespEnc(RespuestaEnc obj)
        {
            int res = 0;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "insert into UX_COM_EncRespuestaEnc (idEncuesta, numEncuesta, idPregEnc, codRest, descPregEnc, respuesta,UABrowser,UAOS,UADevice,UADeviceModel,DescCorta) "
                        + "values (@idEncuesta, @numEncuesta, @idPregEnc, @codRest, @descPregEnc, @respuesta,@UABrowser,@UAOS,@UADevice,@UADeviceModel,@DescCorta)";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@idEncuesta", obj.idEncuesta);
                    cmd.Parameters.AddWithValue("@numEncuesta", obj.numEncuesta);
                    cmd.Parameters.AddWithValue("@idPregEnc", obj.idPregEnc);
                    cmd.Parameters.AddWithValue("@codRest", obj.codRest);
                    cmd.Parameters.AddWithValue("@descPregEnc", obj.descPregEnc);
                    cmd.Parameters.AddWithValue("@respuesta", obj.respuesta);
                    cmd.Parameters.AddWithValue("@UABrowser", obj.userAgent.UABrowser);
                    cmd.Parameters.AddWithValue("@UAOS", obj.userAgent.UAOS);
                    cmd.Parameters.AddWithValue("@UADevice", obj.userAgent.UADevice);
                    cmd.Parameters.AddWithValue("@UADeviceModel", obj.userAgent.UADeviceModel);
                    cmd.Parameters.AddWithValue("@DescCorta", obj.descCorta);

                    cnn.Open();
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    cnn.Dispose();
                    cnn.Close();
                    // return BadRequest(ex.Message);
                    //log
                    DateTime hoy = DateTime.Now;
                    using (StreamWriter writer = new StreamWriter(pathLogs, true))
                    { writer.WriteLine("**Ocurrió un error NewRespEnc() idEncuesta:" + obj.idEncuesta + " NumEncuesta:" + obj.numEncuesta+ " idPregEnc:" + obj.idPregEnc + " CodRest:"+ obj.codRest+ " fecha:" + hoy + " ex:" + ex.ToString()); }
                    //fin 

                }
                finally
                {
                    cnn.Dispose();
                    cnn.Close();
                }
            }

            return res;

        }

        public int GetNumEncuesta(int idEncuesta)//buscar el articulo y obtener los datos
        {
           int num = 0;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                   
                    string sentencia = "select ISNULL(max(NumEncuesta),0) as num from UX_COM_EncRespuestaEnc where idEncuesta=@idEncuesta";                 
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@idEncuesta", idEncuesta);
                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {                      
                       num = Convert.ToInt32(registros["num"]);
                    }
                }
                catch (Exception ex)
                {
                    cnn.Dispose();
                    cnn.Close();
                    //ex.ToString();
                    //log
                    DateTime hoy = DateTime.Now;
                    using (StreamWriter writer = new StreamWriter(pathLogs, true))
                    { writer.WriteLine("**Ocurrió un error GetNumEncuesta() idEncuesta:" + idEncuesta + " fecha:" + hoy + " ex:" + ex.ToString()); }
                    //fin 
                }
                finally
                {
                    cnn.Dispose();
                    cnn.Close();
                }
            }
            if (num == 0)
                num = 1;
            else
                num++;
                
           return num;
        }
    }
}