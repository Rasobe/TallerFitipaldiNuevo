using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TallerFitipaldiNuevo.Clases
{
    public class Pieza
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float Precio { get; set; }
        public int Stock { get; set; }

        public Pieza()
        {
        }

        public Pieza(string nombre, string descripcion, float precio, int stock)
            : this()
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            Stock = stock;
        }

        public override string ToString()
        {
            return $"Pieza: Id={Id}, Nombre={Nombre}, Descripcion={Descripcion}, Precio={Precio}, Stock={Stock}";
        }
    }
}
