using Azure.Core;

namespace Repuestos.API.Models.Entities
{
    public class Marca
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public List<Repuesto> Repuestos { get; set; } = new();
    }
}