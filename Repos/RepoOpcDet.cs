using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Surveys.Repos
{
    public class RepoOpcDet
    {
        string connectionString = Properties.Settings.Default.ConectionString;
        public RepoOpcDet() { }
        public List<OpcDet> GetListAllOpcDet(bool activo, int id, string codRest)
        {
            List<OpcDet> lst = new List<OpcDet>();
            string filtro = "";
            if (!string.IsNullOrWhiteSpace(codRest) && codRest != "0")
            {
                filtro = "and ListaCodRest like '%"+codRest+"%'";
            }
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select * from UX_COM_EncOpcDet where activo=@activo and IdTipoResp=@IdTipoResp "+filtro;
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@IdTipoResp", id);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        OpcDet obj = new OpcDet();//objeto
                        obj.idOpcDet = Convert.ToInt32(registros["idOpcDet"]);
                        obj.idTipoResp = Convert.ToInt32(registros["idTipoResp"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                       // obj.fechaReg = Convert.ToDateTime(registros["fechaReg"]);
                        obj.listaCodRest = registros["listaCodRest"].ToString().Trim();
                        obj.calificacion = Convert.ToInt32(registros["calificacion"]);
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
        public OpcDet GetOpcDet(bool activo, int id)
        {
            OpcDet obj = new OpcDet();//objeto
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select * from UX_COM_EncOpcDet where activo=@activo and IdTipoResp=@IdTipoResp order by IdTipoResp asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@IdTipoResp", id);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                       
                        obj.idTipoResp = Convert.ToInt32(registros["idTipoResp"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                        obj.fechaReg = Convert.ToDateTime(registros["fechaReg"]);
                        obj.listaCodRest = registros["listaCodRest"].ToString().Trim();
                        obj.calificacion = Convert.ToInt32(registros["calificacion"]);

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
        public int GetCalificacion(bool activo, int idTipo, int idOpcDet)
        {
            int cali = 0;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select calificacion from UX_COM_EncOpcDet where activo=@activo and IdTipoResp=@IdTipoResp and idOpcDet=@idOpcDet";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@IdTipoResp", idTipo);
                    cmd.Parameters.AddWithValue("@idOpcDet", idOpcDet);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        cali= Convert.ToInt32(registros["calificacion"]);
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

            return cali;
        }
    }
}