using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Surveys.Repos
{
     class RepoEncuesta
    {
        string connectionString = Properties.Settings.Default.ConectionString;
        public RepoEncuesta() { }

        public List<Encuesta> GetListAllEncuestas(bool activo)
        {
            List<Encuesta> lst = new List<Encuesta>();
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select * from UX_COM_EncEncuesta where activo=@activo order by idEncuesta asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                       Encuesta obj = new Encuesta();//objeto
                        obj.IdEncuesta = Convert.ToInt32(registros["IdEncuesta"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.listaCodRest = registros["listaCodRest"].ToString().Trim();
                        obj.departamento = registros["departamento"].ToString().Trim();
                        obj.fechaIni = Convert.ToDateTime(registros["fechaIni"]);
                        obj.fechaFin = Convert.ToDateTime(registros["fechaFin"]);
                        obj.responsable = registros["responsable"].ToString().Trim();
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                        obj.token = registros["token"].ToString().Trim();
                        obj.urlFondo = registros["urlFondo"].ToString().Trim();
                        obj.subtitulo = registros["subtitulo"].ToString().Trim();
                        obj.mensaCierre = registros["mensaCierre"].ToString().Trim();
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

        public Encuesta GetEncuesta(bool activo, int id, string codRest)
        {
            Encuesta obj = new Encuesta();//objeto
            bool bandera = false;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "";
                    if (!string.IsNullOrWhiteSpace(codRest))
                    {
                        sentencia = "SELECT enc.*, pro.NombreLocal FROM [CSC_COM].[dbo].[UX_COM_EncEncuesta] enc, CSC_DATA.dbo.SAPProyectos pro "
                         + "where activo=@activo and IdEncuesta=@IdEncuesta and enc.ListaCodRest like '%" + codRest + "%' and pro.CodRest = @CodRest";
                        bandera= true;
                    }
                    else
                    {
                        sentencia = "SELECT *, '' NombreLocal  FROM [CSC_COM].[dbo].[UX_COM_EncEncuesta] where activo=@activo and IdEncuesta=@IdEncuesta ";
                    }

                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@IdEncuesta", id);
                    if(bandera)
                    {
                        cmd.Parameters.AddWithValue("@CodRest", codRest);
                    }
                   
                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {                     
                        obj.IdEncuesta = Convert.ToInt32(registros["IdEncuesta"]);
                        obj.descripcion = registros["descripcion"].ToString().Trim();
                        obj.listaCodRest = registros["listaCodRest"].ToString().Trim();
                        obj.departamento = registros["departamento"].ToString().Trim();
                        obj.fechaIni = Convert.ToDateTime(registros["fechaIni"]);
                        obj.fechaFin = Convert.ToDateTime(registros["fechaFin"]);
                        obj.responsable = registros["responsable"].ToString().Trim();
                        obj.activo = Convert.ToBoolean(registros["activo"]);
                        obj.token = registros["token"].ToString().Trim();
                        obj.urlFondo = registros["urlFondo"].ToString().Trim();
                        obj.nombreLocal = registros["nombreLocal"].ToString().Trim();
                        obj.subtitulo = registros["subtitulo"].ToString().Trim();
                        obj.mensaCierre = registros["mensaCierre"].ToString().Trim();
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

        public int ExisteEncuesta(bool activo, string codRest, string ide)
        {
            int existe = 0;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "";
                    if (!string.IsNullOrWhiteSpace(codRest))
                    {
                        sentencia = "select IdEncuesta from UX_COM_EncEncuesta where activo=@activo and token=@token and ListaCodRest like '%" + codRest + "%'";
                    }
                    else
                    {
                        sentencia = "select IdEncuesta from UX_COM_EncEncuesta where activo=@activo and token=@token and ListaCodRest is null "
                            + "or activo=@activo and token=@token and ListaCodRest= '' ";
                    }
                    
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@token", ide);
                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        existe = Convert.ToInt32(registros["IdEncuesta"]);
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

            return existe;
        }
       

    }
}