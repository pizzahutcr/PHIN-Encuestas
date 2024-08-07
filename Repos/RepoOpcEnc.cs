using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Surveys.Repos
{
    public class RepoOpcEnc
    {
        string connectionString = Properties.Settings.Default.ConectionString;
        public RepoOpcEnc() { }

        public List<OpcEnc> GetListAll(bool activo)
        {
            List<OpcEnc> lst = new List<OpcEnc>();
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select * from UX_COM_EncOpcEnc where activo=@activo order by idTipoResp asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        OpcEnc obj = new OpcEnc();//objeto
                        obj.idTipoResp = Convert.ToInt32(registros["idTipoResp"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.componente = registros["componente"].ToString().Trim();
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                        obj.activaCodRest = Convert.ToBoolean(registros["activaCodRest"]);                     
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
        public OpcEnc GetOpcEnc(bool activo, int id)
        {
            OpcEnc obj = new OpcEnc();//objeto
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select * from UX_COM_EncOpcEnc where activo=@activo and idTipoResp=@idTipoResp order by idTipoResp asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@idTipoResp", id);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        
                        obj.idTipoResp = Convert.ToInt32(registros["idTipoResp"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.componente = registros["componente"].ToString().Trim();
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                        obj.activaCodRest = Convert.ToBoolean(registros["activaCodRest"]);
                       // obj.listaOpcDet = GetListAllOpcDet(activo, id);
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
      
    }
}