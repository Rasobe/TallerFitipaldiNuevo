using MySql.Data.MySqlClient;
using System;

namespace TallerFitipaldiNuevo.Clases
{
    internal class CrearBBDD
    {
        private MySqlConnection connection;

        public CrearBBDD()
        {
            string connectionString = "server=localhost;user=root;password=root;";
            this.connection = new MySqlConnection(connectionString);

            try
            {
                this.connection.Open();
                Console.WriteLine("Conexión establecida con éxito.");

                MySqlCommand command = new MySqlCommand("CREATE DATABASE IF NOT EXISTS TallerFitipaldiV", this.connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Base de datos creada o ya existente.");
                CrearTablas();
                InsertarDatosDePrueba();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al crear la base de datos: " + ex.Message);
            }
            finally
            {
                this.connection.Close();
                Console.WriteLine("Conexión cerrada.");
            }
        }

        public void CrearTablas()
        {
            string connectionString = "server=localhost;user=root;password=root;database=TallerFitipaldiV";
            this.connection = new MySqlConnection(connectionString);

            try
            {
                this.connection.Open();
                Console.WriteLine("Conexión establecida con éxito.");

                string createClienteTable = @"
            CREATE TABLE IF NOT EXISTS Cliente (
                id INT PRIMARY KEY AUTO_INCREMENT,
                username VARCHAR(50) NOT NULL,
                password VARCHAR(50) NOT NULL,
                nombre VARCHAR(50) NOT NULL,
                apellidos VARCHAR(50) NOT NULL,
                telefono VARCHAR(20),
                ubicacion VARCHAR(100),
                rol VARCHAR(20)
            );
        ";

                string createVehiculoTable = @"
            CREATE TABLE IF NOT EXISTS Vehiculo (
                matricula VARCHAR(10) PRIMARY KEY,
                modelo VARCHAR(50) NOT NULL,
                marca VARCHAR(50) NOT NULL,
                clienteId INT,
                FOREIGN KEY (clienteId) REFERENCES Cliente(id) ON DELETE CASCADE
            );
        ";

                MySqlCommand command = new MySqlCommand(createClienteTable, this.connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Tabla Cliente creada o ya existente.");

                command.CommandText = createVehiculoTable;
                command.ExecuteNonQuery();
                Console.WriteLine("Tabla Vehiculo creada o ya existente.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al crear las tablas: " + ex.Message);
            }
            finally
            {
                this.connection.Close();
                Console.WriteLine("Conexión cerrada.");
            }
        }

        public void InsertarDatosDePrueba()
        {
            string connectionString = "server=localhost;user=root;password=root;database=TallerFitipaldiV";
            this.connection = new MySqlConnection(connectionString);

            try
            {
                this.connection.Open();
                Console.WriteLine("Conexión establecida con éxito.");

                // Verificar si ya existen registros en la tabla Cliente
                string selectCliente = "SELECT COUNT(*) FROM Cliente";
                MySqlCommand selectCommand = new MySqlCommand(selectCliente, this.connection);
                object result = selectCommand.ExecuteScalar();
                int count = Convert.ToInt32(result);

                // Si no se encuentran registros en la tabla Cliente, insertar datos de prueba
                if (count == 0)
                {

                    // Insertar datos de prueba en la tabla Cliente
                    string insertCliente = @"
            INSERT INTO Cliente (username, password, nombre, apellidos, telefono, ubicacion, rol)
            VALUES
                ('cliente1', 'password1', 'Nombre1', 'Apellido1', '123456789', 'Ubicacion1', 'ADMIN'),
                ('cliente2', 'password2', 'Nombre2', 'Apellido2', '234567890', 'Ubicacion2', 'ADMIN'),
                ('cliente3', 'password3', 'Nombre3', 'Apellido3', '345678901', 'Ubicacion3', 'USER'),
                ('cliente4', 'password4','Nombre4','Apellido4','456789012','Ubicacion4','USER'),
                ('cliente5','password5','Nombre5','Apellido5','567890123','Ubicacion5','USER');
        ";
                    MySqlCommand command = new MySqlCommand(insertCliente, this.connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Datos de prueba insertados en la tabla Cliente.");
                }

                // Verificar si ya existen registros en la tabla Cliente.
                selectCliente = "SELECT COUNT(*) FROM Vehiculo";
                selectCommand = new MySqlCommand(selectCliente, this.connection);
                result = selectCommand.ExecuteScalar();
                count = Convert.ToInt32(result);

                // Si no se encuentran registros en la tabla Cliente, insertar datos de prueba
                if (count == 0)
                {

                    // Insertar datos de prueba en la tabla Vehiculo
                    string insertVehiculo = @"
            INSERT INTO Vehiculo (matricula, modelo, marca, clienteId)
            VALUES
                ('AAA1111','Modelo1','Marca1',(SELECT id FROM Cliente WHERE username='cliente1')),
                ('BBB2222','Modelo2','Marca2',(SELECT id FROM Cliente WHERE username='cliente1')),
                ('CCC3333','Modelo3','Marca3',(SELECT id FROM Cliente WHERE username='cliente2')),
                ('DDD4444','Modelo4','Marca4',(SELECT id FROM Cliente WHERE username='cliente3')),
                ('EEE5555','Modelo5','Marca5',(SELECT id FROM Cliente WHERE username='cliente3'));
        ";
                    MySqlCommand command = new MySqlCommand(insertVehiculo, this.connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Datos de prueba insertados en la tabla Vehiculo.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
