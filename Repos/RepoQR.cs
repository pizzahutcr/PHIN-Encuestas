using Antlr.Runtime.Misc;
using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveys.Repos
{
    public class RepoQR
    {
        string connectionString = Properties.Settings.Default.ConectionString;
        public RepoQR() { }
        public List<SelectListItem> GetOptionsEncuestas()
        {
           
            List<SelectListItem> lst = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select token, Descripcion from UX_COM_EncEncuesta where activo=1 order by idEncuesta asc", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lst.Add(new SelectListItem() { Text = reader["Descripcion"].ToString(), Value = reader["token"].ToString() });  
                        }
                    }
                }
                conn.Dispose();
                conn.Close();
            }

            return lst;
        }

        public List<SelectListItem> GetOptionsLocales(string lista)
        {
            List<SelectListItem> lst = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //using (SqlCommand cmd = new SqlCommand("select CodRest, NombreLocal from CSC_DATA.dbo.SAPProyectos where TipoProyecto='Operativo' order by NombreLocal desc", conn))
                using (SqlCommand cmd = new SqlCommand("select CodRest, NombreLocal from CSC_DATA.dbo.SAPProyectos where TipoProyecto='Operativo' and CodRest in (@lista) order by NombreLocal asc", conn))
                {
                    cmd.Parameters.AddWithValue("@lista", lista);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lst.Add(new SelectListItem() { Text = reader["NombreLocal"].ToString(), Value = reader["CodRest"].ToString() });
                        }
                    }
                }
                conn.Dispose();
                conn.Close();
            }

            //string sentencia = "select * from UX_COM_EncEncuesta where activo=@activo order by idEncuesta asc";
            //SqlCommand cmd = new SqlCommand(sentencia, cnn);
            //cmd.Parameters.AddWithValue("@activo", activo);

            return lst;
        }
        public string GetListCodRest(bool activo, string token)
        {
            string listaCodRest = "";
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select ListaCodRest from UX_COM_EncEncuesta where activo=@activo and token=@token order by idEncuesta asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    cmd.Parameters.AddWithValue("@activo", activo);
                    cmd.Parameters.AddWithValue("@token", token);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        listaCodRest = registros["listaCodRest"].ToString().Trim();
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

            return listaCodRest;
        }

        //****************************************************************************
        //************ v2 ************************************************************
        public List<QRLocales> GetListLocales(string lista)
        {
            List<QRLocales> lst = new List<QRLocales>();

            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    //using (SqlCommand cmd = new SqlCommand("select CodRest, NombreLocal from CSC_DATA.dbo.SAPProyectos where TipoProyecto='Operativo' order by NombreLocal desc", conn))
            //    using (SqlCommand cmd = new SqlCommand("select CodRest, NombreLocal from CSC_DATA.dbo.SAPProyectos where TipoProyecto='Operativo' and CodRest in (@lista) order by NombreLocal asc", conn))
            //    {
            //        cmd.Parameters.AddWithValue("@lista", lista);
            //        using (SqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                //lst.Add(new SelectListItem() { Text = reader["NombreLocal"].ToString(), Value = reader["CodRest"].ToString() });
            //                QRLocales obj = new QRLocales();
            //                obj.nombre = reader["NombreLocal"].ToString();
            //                obj.codRest = reader["CodRest"].ToString();
            //                lst.Add(obj);
            //            }
            //        }
            //    }
            //}

            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                try
                {
                    string sentencia = "select CodRest, NombreLocal from CSC_DATA.dbo.SAPProyectos where TipoProyecto='Operativo' and CodRest in ("+lista+") order by NombreLocal asc";
                    SqlCommand cmd = new SqlCommand(sentencia, cnn);
                    //cmd.Parameters.AddWithValue("@lista", lista);

                    cnn.Open();
                    SqlDataReader registros = cmd.ExecuteReader();
                    while (registros.Read())
                    {
                        QRLocales obj = new QRLocales();
                        obj.codRest = registros["CodRest"].ToString().Trim();
                        obj.nombre = registros["NombreLocal"].ToString().Trim();                       
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
        public List<QREncuesta> GetListEncuestas()
        {

            List<QREncuesta> lst = new List<QREncuesta>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select token, Descripcion from UX_COM_EncEncuesta where activo=1 order by idEncuesta asc", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //lst.Add(new SelectListItem() { Text = reader["Descripcion"].ToString(), Value = reader["token"].ToString() });
                            QREncuesta encuesta = new QREncuesta();
                            encuesta.nombre = reader["Descripcion"].ToString();
                            encuesta.idEncuesta = reader["token"].ToString();
                            lst.Add(encuesta);
                        }
                    }
                }
                conn.Dispose();
                conn.Close();
            }

            return lst;
        }

        //***fin v2

    }
}