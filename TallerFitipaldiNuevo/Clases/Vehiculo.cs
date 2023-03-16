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
        private int id;
        private string matricula;
        private string modelo;
        private string marca;

        [ForeignKey("Cliente")]
        private int clienteId;
        private Cliente cliente;

        public Vehiculo()
        {
        }

        public Vehiculo(string matricula, string modelo, string marca, int clienteId)
        {
            this.matricula = matricula;
            this.modelo = modelo;
            this.marca = marca;
            this.clienteId = clienteId;
        }

        public int Id { get => id; set => id = value; }
        public string Matricula { get => matricula; set => matricula = value; }
        public string Modelo { get => modelo; set => modelo = value; }
        public string Marca { get => marca; set => marca = value; }
        public int ClienteId { get => clienteId; set => clienteId = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }

        public override string ToString()
        {
            return $"Vehiculo: Id = {id}, Matricula = {matricula}, Modelo = {modelo}, Marca = {marca}, ClienteId = {clienteId}";
        }
    }
}
