namespace CapaNegocio
{
    public class Gasto
    {
        public int IdGasto { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCategoria { get; set; }
    }
}
