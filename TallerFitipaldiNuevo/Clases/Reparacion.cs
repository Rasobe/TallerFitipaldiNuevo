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
        public decimal PrecioPorHora { get; set; }
        public decimal PrecioSinIva { get; set; }
        public decimal Iva { get; set; }

        public DateTime DiaInicioReparacion { get; set; }

        [NotMapped]
        public decimal PrecioTotalPiezas => PrecioSinIva + (PrecioSinIva * Iva / 100);
        [NotMapped]
        public decimal PrecioTotalHoras => Horas * PrecioPorHora;

        [ForeignKey("Mecanico")]
        public int MecanicoId { get; set; }
        public virtual Cliente Mecanico { get; set; }
        public bool Finalizado { get; set; }
        public Reparacion() { }

        public Reparacion(int vehiculoId, decimal horas, decimal precioPorHora, decimal precioSinIva, decimal iva, DateTime diaInicioReparacion, int mecanicoId) : this()
        {
            VehiculoId = vehiculoId;
            Horas = horas;
            PrecioPorHora = precioPorHora;
            PrecioSinIva = precioSinIva;
            Iva = iva;
            DiaInicioReparacion = diaInicioReparacion;
            MecanicoId = mecanicoId;
            Finalizado = false;
        }

        public override string ToString()
        {
            return $"Id: {Id}, VehiculoId: {VehiculoId}, DiaInicioReparacion: {DiaInicioReparacion}, Horas: {Horas}, PrecioPorHora: {PrecioPorHora}, PrecioSinIva: {PrecioSinIva}, Iva: {Iva}, DiaInicioReparacion: {DiaInicioReparacion.ToShortDateString()}, PrecioTotalPiezas: {PrecioTotalPiezas}, MecanicoId: {MecanicoId}, Finalizado: {Finalizado}";
        }

    }
}
