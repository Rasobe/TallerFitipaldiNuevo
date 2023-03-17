using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerFitipaldiNuevo.Clases
{
    public class Vehiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Matricula { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<Pieza> Piezas { get; set; }

        public Vehiculo()
        {
            Piezas = new HashSet<Pieza>();
        }

        public Vehiculo(string matricula, string tipo, string modelo, string marca, int clienteId)
            : this()
        {
            Tipo = tipo;
            Matricula = matricula;
            Modelo = modelo;
            Marca = marca;
            ClienteId = clienteId;
        }

        public override string ToString()
        {
            return $"Vehiculo: Tipo = {Tipo}, Matricula = {Matricula}, Modelo = {Modelo}, Marca = {Marca}";
        }
    }
}
