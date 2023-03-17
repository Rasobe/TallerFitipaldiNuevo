using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerFitipaldiNuevo.Clases
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Ubicacion { get; set; }
        public string Rol { get; set; }

        // Propiedad de navegación para la relación One2Many
        public virtual ICollection<Vehiculo> Vehiculos { get; set; }

        public Cliente()
        {
            // Inicializamos la lista de vehículos
            this.Vehiculos = new HashSet<Vehiculo>();
        }

        public Cliente(int id, string username, string password, string nombre, string apellidos, string telefono, string ubicacion, string rol)
        {
            Id = id;
            Username = username;
            Password = password;
            Nombre = nombre;
            Apellidos = apellidos;
            Telefono = telefono;
            Ubicacion = ubicacion;
            Rol = rol;
        }

        public Cliente(string username, string password, string nombre, string apellidos, string telefono, string ubicacion, string rol)
        {
            Username = username;
            Password = password;
            Nombre = nombre;
            Apellidos = apellidos;
            Telefono = telefono;
            Ubicacion = ubicacion;
            Rol = rol;

            // Inicializamos la lista de vehículos
            this.Vehiculos = new HashSet<Vehiculo>();
        }

        public Cliente(string username, string password, string nombre, string apellidos, string telefono, string ubicacion)
        {
            Username = username;
            Password = password;
            Nombre = nombre;
            Apellidos = apellidos;
            Telefono = telefono;
            Ubicacion = ubicacion;
        }

        public override string ToString()
        {
            return $"Cliente: [Id: {Id}, Username: {Username}, Password: {Password}, Nombre: {Nombre}, Apellidos: {Apellidos}, Telefono: {Telefono}, Ubicacion: {Ubicacion}, Rol: {Rol}]";
        }

    }
}
