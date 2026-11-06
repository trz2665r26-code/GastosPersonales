using System.Data;
using System.Globalization;
using CapaDatos;

namespace CapaNegocio
{
    public class GastoNegocio
    {
        private GastoDatos datos = new GastoDatos();

        // Convierte lo que escribió el usuario en un Gasto y aplica las reglas de negocio.
        // Si algo no cumple, lanza una excepción con el mensaje para el usuario.
        private Gasto Preparar(int idGasto, string descripcion, string montoTexto, DateTime fecha, object idCategoria)
        {
            Gasto g = new Gasto();
            g.IdGasto = idGasto;

            // Regla 2: la descripción es obligatoria
            if (descripcion == null || descripcion.Trim() == "")
            {
                throw new Exception("La descripción es obligatoria.");
            }
            g.Descripcion = descripcion.Trim();

            // Regla 1: el monto debe ser mayor que 0
            decimal monto;
            if (!decimal.TryParse(montoTexto, NumberStyles.Number, CultureInfo.CurrentCulture, out monto))
            {
                throw new Exception("El monto debe ser un número válido.");
            }
            if (monto <= 0)
            {
                throw new Exception("El monto debe ser mayor que 0.");
            }
            g.Monto = monto;

            // Regla 3: la fecha no puede ser futura
            if (fecha.Date > DateTime.Today)
            {
                throw new Exception("La fecha no puede ser futura.");
            }
            g.Fecha = fecha.Date;

            if (idCategoria == null)
            {
                throw new Exception("Seleccione una categoría.");
            }
            g.IdCategoria = Convert.ToInt32(idCategoria);

            return g;
        }

        private void ValidarMesAnio(int mes, int anio)
        {
            if (mes < 1 || mes > 12)
            {
                throw new Exception("El mes debe estar entre 1 y 12.");
            }
            if (anio < 2000 || anio > 2100)
            {
                throw new Exception("El año no es válido.");
            }
        }

        public void Agregar(string descripcion, string montoTexto, DateTime fecha, object idCategoria)
        {
            Gasto g = Preparar(0, descripcion, montoTexto, fecha, idCategoria);
            datos.Insertar(g.Descripcion, g.Monto, g.Fecha, g.IdCategoria);
        }

        public void Modificar(int idGasto, string descripcion, string montoTexto, DateTime fecha, object idCategoria)
        {
            if (idGasto <= 0)
            {
                throw new Exception("Seleccione un gasto de la lista para modificarlo.");
            }
            Gasto g = Preparar(idGasto, descripcion, montoTexto, fecha, idCategoria);
            datos.Actualizar(g.IdGasto, g.Descripcion, g.Monto, g.Fecha, g.IdCategoria);
        }

        public void Eliminar(int idGasto)
        {
            if (idGasto <= 0)
            {
                throw new Exception("Seleccione un gasto de la lista para eliminarlo.");
            }
            datos.Eliminar(idGasto);
        }

        public DataTable Listar()
        {
            return datos.Listar();
        }

        public DataTable ListarPorMesAnio(int mes, int anio)
        {
            ValidarMesAnio(mes, anio);
            return datos.ListarPorMesAnio(mes, anio);
        }

        public DataTable TotalMesPorCategoria(int mes, int anio)
        {
            ValidarMesAnio(mes, anio);
            return datos.TotalPorCategoria(mes, anio);
        }

        public DataTable MayorGastoDelMes(int mes, int anio)
        {
            ValidarMesAnio(mes, anio);
            return datos.MayorGastoDelMes(mes, anio);
        }
    }
}
