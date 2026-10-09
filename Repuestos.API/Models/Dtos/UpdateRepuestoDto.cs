namespace Repuestos.API.Models.Dtos
{
    public class UpdateRepuestoDto
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public int MarcaId { get; set; }
    }
}