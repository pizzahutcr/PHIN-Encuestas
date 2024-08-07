using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Surveys.Repos
{
    public class RepoPreguntasEnc
    {
        string connectionString = Properties.Settings.Default.ConectionString;
        public RepoPreguntasEnc() { }

        public List<PreguntasEnc> GetListAll(bool activo, int id, string codRest)
        {
            List<PreguntasEnc> lst = new List<PreguntasEnc>();
            string filtro = "";
            if (!string.IsNullOrWhiteSpace(codRest) && codRest != "0")
            {
                filtro = "and ListaCodRest like '%" + codRest + "%' ";
            }
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select * from UX_COM_EncPreguntasEnc where activo=@activo and idEncuesta=@idEncuesta "+ filtro +"order by numPregunta asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@idEncuesta", id);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        PreguntasEnc obj = new PreguntasEnc();//objeto
                        obj.IdPregEnc = Convert.ToInt32(registros["IdPregEnc"]);
                        obj.IdEncuesta = Convert.ToInt32(registros["IdEncuesta"]);
                        obj.numPregunta = Convert.ToInt32(registros["numPregunta"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.idTipoResp = Convert.ToInt32(registros["idTipoResp"]);                      
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                        obj.fechaReg = Convert.ToDateTime(registros["fechaReg"].ToString().Trim());
                        obj.listaCodRest = registros["listaCodRest"].ToString().Trim();

                        lst.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    cnn.Dispose();
                    cnn.Close();
                    // return BadRequest(ex.Message);

                }
                finally
                {
                    cnn.Dispose();
                    cnn.Close();
                }
            }

            return lst;
        }
        public PreguntasEnc GetPregEnc(bool activo, int id)
        {
            PreguntasEnc obj = new PreguntasEnc();//objeto
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select * from UX_COM_EncPreguntasEnc where activo=@activo and idEncuesta=@idEncuesta order by numPregunta asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@idEncuesta", id);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {                     
                        obj.IdEncuesta = Convert.ToInt32(registros["IdEncuesta"]);
                        obj.numPregunta = Convert.ToInt32(registros["numPregunta"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.idTipoResp = Convert.ToInt32(registros["idTipoResp"]);
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                        obj.fechaReg = Convert.ToDateTime(registros["fechaReg"].ToString().Trim());
                        obj.listaCodRest = registros["listaCodRest"].ToString().Trim();
                    }
                }
                catch (Exception ex)
                {
                    cnn.Dispose();
                    cnn.Close();
                    // return BadRequest(ex.Message);

                }
                finally
                {
                    cnn.Dispose();
                    cnn.Close();
                }
            }

            return obj;
        }
        public string[] GetPregEncDesc(bool activo, int idEncuesta, int idPregEnc)
        {
            string[] pregunta= {"",""};
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select Descripcion, descCorta from UX_COM_EncPreguntasEnc where activo=@activo and idEncuesta=@idEncuesta and idPregEnc=@idPregEnc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@idEncuesta", idEncuesta);
                    cmd.Parameters.AddWithValue("@idPregEnc", idPregEnc);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        pregunta[0] = registros["Descripcion"].ToString().Trim();
                        pregunta[1] = registros["descCorta"].ToString().Trim();                     
                    }
                }
                catch (Exception ex)
                {
                    cnn.Dispose();
                    cnn.Close();
                    // return BadRequest(ex.Message);

                }
                finally
                {
                    cnn.Dispose();
                    cnn.Close();
                }
            }

            return pregunta;
        }
    }
}