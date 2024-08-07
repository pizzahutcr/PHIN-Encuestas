using Antlr.Runtime.Misc;
using Antlr.Runtime.Tree;
using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Surveys.Repos
{
    public class RepoRespuestaDet
    {
        string connectionString = Properties.Settings.Default.ConectionString;
        string pathLogs = Properties.Settings.Default.PathLog;//para los logs
        public RepoRespuestaDet() { }

        public int NewRespDet(RespuestaDet obj)
        {
            int res = 0;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "insert into UX_COM_EncRespuestaDet (idEncuesta, numEncuesta, idPregDet, codRest, descPregDet, respuesta, calificacion,descCorta) "
                        + "values (@idEncuesta, @numEncuesta, @idPregDet, @codRest, @descPregDet, @respuesta, @calificacion,@descCorta)";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@idEncuesta", obj.idEncuesta);
                    cmd.Parameters.AddWithValue("@numEncuesta", obj.numEncuesta);
                    cmd.Parameters.AddWithValue("@idPregDet", obj.idPregDet);
                    cmd.Parameters.AddWithValue("@codRest", obj.codRest);
                    cmd.Parameters.AddWithValue("@descPregDet", obj.descPregDet);
                    cmd.Parameters.AddWithValue("@respuesta", obj.respuesta);
                    cmd.Parameters.AddWithValue("@calificacion", obj.calificacion);
                    cmd.Parameters.AddWithValue("@descCorta", obj.descCorta);

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
                    { writer.WriteLine("**Ocurrió un error NewRespDet() idEncuesta:" + obj.idEncuesta + " NumEncuesta:" + obj.numEncuesta+ " idPregDet:" + obj.idPregDet + " CodRest:"+ obj.codRest+ " fecha:" + hoy + " ex:" + ex.ToString()); }
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

        public int GetNumEncuesta(int idEncuesta, string codRest)//buscar el articulo y obtener los datos
        {
           int num = 0;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                   
                    string sentencia = "select ISNULL(max(NumEncuesta),0) as num from UX_COM_EncRespuestaDet where idEncuesta=@idEncuesta and codRest=@codRest";                 
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@idEncuesta", idEncuesta);
                    cmd.Parameters.AddWithValue("@codRest", codRest);
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
                    { writer.WriteLine("**Ocurrió un error GetNumEncuesta() idEncuesta:" + idEncuesta + " CodRest:" + codRest + " fecha:" + hoy + " ex:" + ex.ToString()); }
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