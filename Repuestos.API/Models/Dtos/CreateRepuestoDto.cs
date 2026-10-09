namespace Repuestos.API.Models.Dtos
{
    public class CreateRepuestoDto
    {
        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public int MarcaId { get; set; }
    }
}