using System.Data;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public static class Conexion
    {
        // Única cadena de conexión. Cambie "localhost" por el nombre de su servidor
        // (ej: .\SQLEXPRESS o (localdb)\MSSQLLocalDB).
        private static readonly string cadena =
            "Server=(localdb)\\MSSQLLocalDB;Database=GastosPersonalesDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadena);
        }

        // Ejecuta un SELECT y devuelve un DataTable.
        public static DataTable Consultar(string sql, params SqlParameter[] parametros)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection cn = ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(parametros);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(tabla);
                }
            }
            return tabla;
        }

        // Ejecuta INSERT, UPDATE o DELETE y devuelve las filas afectadas.
        public static int Ejecutar(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection cn = ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(parametros);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
