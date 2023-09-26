using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using appSCISA.Models;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

namespace appSCISA.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source=localHost;Initial Catalog=proyectoSCISA; User=sa; Password=dixon222669117";
        
        //GET Acceso
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(Usuarios oUsuarios)
        {
            bool registrado;
            string mensaje;

            if (oUsuarios.Contraseña == oUsuarios.ConfirmarClave)
            {

                oUsuarios.Contraseña = Encrypt(oUsuarios.Contraseña);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.Parameters.AddWithValue("Nombre", oUsuarios.Nombre);
                cmd.Parameters.AddWithValue("CorreoElectronico", oUsuarios.CorreoElectronico);
                cmd.Parameters.AddWithValue("Contraseña", oUsuarios.Contraseña);
                cmd.Parameters.AddWithValue("Rol", "Cliente");
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();


            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login(Usuarios oUsuarios)
        {
            oUsuarios.Contraseña = Encrypt(oUsuarios.Contraseña);

            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("CorreoElectronico", oUsuarios.CorreoElectronico);
                cmd.Parameters.AddWithValue("Contraseña", oUsuarios.Contraseña);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                oUsuarios.UserID = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }

            if (oUsuarios.UserID != 0)
            {
            TempData["usuario"] = oUsuarios.UserID;
                return RedirectToAction("Ingresar", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "usuario no encontrado";
                return View();
            }
        }

        //ENCRIPTAR MD5
        public string Encrypt(string mensaje)
        {
            string hash = "encriptacion SCISA";
            byte[] data = UTF8Encoding.UTF8.GetBytes(mensaje);

            MD5 md5 = MD5.Create();
            TripleDES tripldes = TripleDES.Create();

            tripldes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripldes.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripldes.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }

        //Desencriptar
        public string Decrypt(string mensajeEn)
        {
            string hash = "encriptacion SCISA";
            byte[] data = Convert.FromBase64String(mensajeEn);

            MD5 md5 = MD5.Create();
            TripleDES tripldes = TripleDES.Create();

            tripldes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripldes.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripldes.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }


    }
}