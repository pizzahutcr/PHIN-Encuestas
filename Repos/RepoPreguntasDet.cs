using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Surveys.Repos
{
    public class RepoPreguntasDet
    {
        string connectionString = Properties.Settings.Default.ConectionString;
        public RepoPreguntasDet() { }

        public List<PreguntasDet> GetListAll(bool activo, int id, string codRest)
        {
            List<PreguntasDet> lst = new List<PreguntasDet>();
            string filtro = "";
            if (!string.IsNullOrWhiteSpace(codRest) && codRest != "0")
            {
                filtro = "and ListaCodRest like '%" + codRest + "%' ";
            }
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select * from UX_COM_EncPreguntasDet where activo=@activo and idEncuesta=@idEncuesta "+filtro+"order by numPregunta asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@idEncuesta", id);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        PreguntasDet obj = new PreguntasDet();//objeto
                        obj.IdPregunta = Convert.ToInt32(registros["IdPregunta"]);
                        obj.IdEncuesta = Convert.ToInt32(registros["IdEncuesta"]);
                        obj.numPregunta = Convert.ToInt32(registros["numPregunta"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.idTipoResp = Convert.ToInt32(registros["idTipoResp"]);                       
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                        obj.fechaReg = Convert.ToDateTime(registros["fechaReg"].ToString().Trim());
                        obj.obligatorio = Convert.ToBoolean(registros["obligatorio"]);
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
        public string[] GetPregDetDesc(bool activo, int idEncuesta, int idPregunta)
        {
            string[] pregunta = { "", "" };
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select Descripcion, descCorta from UX_COM_EncPreguntasDet where activo=@activo and idEncuesta=@idEncuesta and idPregunta=@idPregunta";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@idEncuesta", idEncuesta);
                    cmd.Parameters.AddWithValue("@idPregunta", idPregunta);

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