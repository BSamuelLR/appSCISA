using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using appSCISA.Models;
using appSCISA.Permisos;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient;


namespace appSCISA.Controllers
{
    //[ValidarSesion]
    public class HomeController : Controller
    {
        static string cadena = "Data Source=localHost;Initial Catalog=proyectoSCISA; User=sa; Password=dixon222669117";

        public IActionResult Index()
        {
            var modelo = MostrarProductos();
            return View(modelo);
        }
        

        public IActionResult MostrarProductos()
        {
            List<Producto> productos = new List<Producto>();

            string query = "SELECT * FROM Productos";

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto producto = new Producto
                            {
                                ProductoID = (int)reader["ProductoID"],
                                TipoProducto = reader["TipoProducto"].ToString(),
                                NombreProducto = reader["NombreProducto"].ToString(),
                                EstadoProducto = reader["EstadoProducto"].ToString(),
                                FechaIngreso = (DateTime)reader["FechaIngreso"],
                                ValorCalculado = (decimal)reader["ValorCalculado"],
                                TiempoVencimientoDevolucion = (TimeSpan)reader["TiempoVencimientoDevolucion"]
                            };

                            productos.Add(producto);
                        }
                    }
                }
            }

            return View(productos);
        }

        public IActionResult ingresar()
        {
            return RedirectToAction("Login", "Acceso");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
