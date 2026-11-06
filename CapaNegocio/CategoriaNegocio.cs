using System.Data;
using CapaDatos;

namespace CapaNegocio
{
    public class CategoriaNegocio
    {
        private CategoriaDatos datos = new CategoriaDatos();

        public DataTable Listar()
        {
            return datos.Listar();
        }
    }
}
