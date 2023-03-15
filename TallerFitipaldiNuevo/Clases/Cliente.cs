using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerFitipaldiNuevo.Clases
{
    internal class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int id;
        private String username;
        private String password;
        private String nombre;
        private String apellidos;
        private String telefono;
        private String ubicacion;
        private String rol;

        // Propiedad de navegación para la relación One2Many
        public virtual ICollection<Vehiculo> Vehiculos { get; set; }

        public Cliente()
        {
            // Inicializamos la lista de vehículos
            this.Vehiculos = new HashSet<Vehiculo>();
        }

        public Cliente(string username, string password, string nombre, string apellidos, string telefono, string ubicacion, string rol)
        {
            this.username = username;
            this.password = password;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.telefono = telefono;
            this.ubicacion = ubicacion;
            this.rol = rol;

            // Inicializamos la lista de vehículos
            this.Vehiculos = new HashSet<Vehiculo>();
        }

        public Cliente(string username, string password, string nombre, string apellidos, string telefono, string ubicacion)
        {
            this.username = username;
            this.password = password;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.telefono = telefono;
            this.ubicacion = ubicacion;
        }

        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellidos { get => apellidos; set => apellidos = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Ubicacion { get => ubicacion; set => ubicacion = value; }
        public string Rol { get => rol; set => rol = value; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
