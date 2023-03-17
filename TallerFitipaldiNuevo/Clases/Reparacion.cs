using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerFitipaldiNuevo.Clases
{
    public class Reparacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Vehiculo")]
        public int VehiculoId { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }

        public decimal Horas { get; set; }
        public decimal PrecioSinIva { get; set; }
        public decimal Iva { get; set; }

        public DateTime DiaInicioReparacion { get; set; }

        [NotMapped]
        public decimal PrecioTotal => PrecioSinIva + (PrecioSinIva * Iva / 100);

        [ForeignKey("Mecanico")]
        public int MecanicoId { get; set; }
        public virtual Cliente Mecanico { get; set; }
        public bool Finalizado { get; set; }
        public Reparacion() { }

        public Reparacion(int vehiculoId, decimal horas, decimal precioSinIva, decimal iva, int mecanicoId) : this()
        {
            VehiculoId = vehiculoId;
            Horas = horas;
            PrecioSinIva = precioSinIva;
            Iva = iva;
            MecanicoId = mecanicoId;
        }

        public override string ToString()
        {
            return $"Id: {Id}, VehiculoId: {VehiculoId}, Horas: {Horas}, PrecioSinIva: {PrecioSinIva}, Iva: {Iva}, DiaInicioReparacion: {DiaInicioReparacion.ToShortDateString()}, PrecioTotal: {PrecioTotal}, MecanicoId: {MecanicoId}, Finalizado: {Finalizado}";
        }

    }
}
