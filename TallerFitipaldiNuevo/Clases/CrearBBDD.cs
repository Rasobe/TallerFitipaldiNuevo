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
                    Id INT PRIMARY KEY AUTO_INCREMENT,
                    Username VARCHAR(50) NOT NULL UNIQUE,
                    Password VARCHAR(50) NOT NULL,
                    Nombre VARCHAR(50) NOT NULL,
                    Apellidos VARCHAR(50) NOT NULL,
                    Telefono VARCHAR(20),
                    Ubicacion VARCHAR(100),
                    Rol VARCHAR(20)
                );
            ";

                string createVehiculoTable = @"
                CREATE TABLE IF NOT EXISTS Vehiculo (
                    Id INT PRIMARY KEY AUTO_INCREMENT,
                    Matricula VARCHAR(10) NOT NULL UNIQUE,
                    Tipo VARCHAR(50) NOT NULL,
                    Modelo VARCHAR(50) NOT NULL,
                    Marca VARCHAR(50) NOT NULL,
                    ClienteId INT,
                    FOREIGN KEY (clienteId) REFERENCES Cliente(id) ON DELETE CASCADE ON UPDATE CASCADE
                );
            ";

                string createPiezasTable = @"
                CREATE TABLE IF NOT EXISTS Pieza (
                    Id INT PRIMARY KEY AUTO_INCREMENT,
                    Nombre VARCHAR(50) NOT NULL UNIQUE,
                    Descripcion VARCHAR(255) NOT NULL,
                    Stock INT NOT NULL,
                    Precio decimal(10,2) NOT NULL
                );
            ";

                string createReparacionesTable = @"
                CREATE TABLE IF NOT EXISTS Reparacion (
                    Id INTEGER PRIMARY KEY AUTO_INCREMENT,
                    VehiculoId INTEGER NOT NULL,
                    Horas DECIMAL NOT NULL,
                    PrecioPorHora DECIMAL NOT NULL,
                    PrecioTotalHoras DECIMAL(10,2) AS (Horas * PrecioPorHora),
                    PrecioSinIva DECIMAL NOT NULL,
                    Iva DECIMAL NOT NULL,
                    PrecioTotalPiezas DECIMAL(10,2) AS (PrecioSinIva + (PrecioSinIva * Iva / 100)),
                    PrecioTotal DECIMAL(10,2) AS (precioTotalPiezas + precioTotalHoras),
                    DiaInicioReparacion DATETIME NOT NULL,
                    MecanicoId INTEGER NOT NULL,
                    Finalizado BOOLEAN NOT NULL,
                    FOREIGN KEY (vehiculoId) REFERENCES Vehiculo(id) ON DELETE CASCADE ON UPDATE CASCADE, 
                    FOREIGN KEY (mecanicoId) REFERENCES Cliente(id) ON DELETE CASCADE ON UPDATE CASCADE
                );
            ";

                string createReparacionPiezasTable = @"
                CREATE TABLE PiezaReparacion (
                    ReparacionId INTEGER NOT NULL,
                    PiezaId INTEGER NOT NULL,
                    Cantidad int NOT NULL,
                    PRIMARY KEY (ReparacionId, PiezaId),
                    FOREIGN KEY (ReparacionId) REFERENCES Reparacion(Id) ON DELETE CASCADE ON UPDATE CASCADE,
                    FOREIGN KEY (PiezaId) REFERENCES Pieza(Id) ON DELETE CASCADE ON UPDATE CASCADE
                );
            ";

                MySqlCommand command = new MySqlCommand(createClienteTable, this.connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Tabla Cliente creada o ya existente.");

                command.CommandText = createVehiculoTable;
                command.ExecuteNonQuery();
                Console.WriteLine("Tabla Vehiculo creada o ya existente.");

                command.CommandText = createPiezasTable;
                command.ExecuteNonQuery();
                Console.WriteLine("Tabla Pieza creada o ya existente.");

                command.CommandText = createReparacionesTable;
                command.ExecuteNonQuery();
                Console.WriteLine("Tabla Reparaciones creada o ya existente.");

                command.CommandText = createReparacionPiezasTable;
                command.ExecuteNonQuery();
                Console.WriteLine("Tabla relacion Reparacion / Piezas creada o ya existente.");

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
                        ('cliente1', 'password1', 'Juan', 'Pérez', '+34 123456789', 'Madrid, España', 'ADMIN'), 
                        ('cliente2', 'password2', 'María', 'García', '+34 234567890', 'Barcelona, España', 'MECANICO'), 
                        ('cliente3', 'password3', 'Carlos', 'Fernández','+34 345678901','Valencia, España','USER'), 
                        ('cliente4', 'password4','Ana','Rodríguez','+34 456789012','Sevilla, España','USER'), 
                        ('cliente5', 'password5','David','González','+34 567890123','Zaragoza, España','USER'),
                        ('cliente6', 'password6', 'Juan', 'Pérez', '+34 123456789', 'Madrid', 'USER'),
                        ('cliente7', 'password7', 'María', 'García', '+34 234567890', 'Barcelona','MECANICO'),
                        ('cliente8', 'password8', 'Carlos','Fernández','+34 345678901','Valencia','USER'),
                        ('cliente9', 'password9','Ana','Rodríguez','+34 456789012','Sevilla','USER'),
                        ('cliente10', 'password10','David','González','+34 567890123','Zaragoza','USER'),
                        ('cliente11', 'password11', 'Lucía', 'Martínez', '+34 678901234', 'Bilbao, España', 'ADMIN'),
                        ('cliente12', 'password12', 'Javier', 'López', '+34 789012345', 'Málaga, España', 'ADMIN'),
                        ('cliente13', 'password13','Sofía','Gómez','+34 890123456','Alicante, España','USER'),
                        ('cliente14', 'password14','Miguel','Sánchez','+34 901234567','Córdoba, España','MECANICO'),
                        ('cliente15', 'password15','Laura','Romero','+34 012345678','Valladolid, España' ,'USER'),
                        ('cliente16', 'password16', 'Daniel', 'Alonso' ,'+34 123456780' ,'Vigo ,España' ,'USER'),
                        ('cliente17', 'password17' ,'Isabel' ,'Castro' ,'+34 234567801' ,'Gijón ,España' ,'USER'),
                        ('cliente18', 'password18' ,'Sergio' ,'Ortega' ,'+34 345678012 ','Granada ,España ','USER'),
                        ('cliente19', 'password19 ','Elena ','Torres ','+34 456780123 ','Oviedo ,España ','MECANICO'),
                        ('cliente20', 'password20', 'Carmen', 'Gutiérrez', '+34 678901235', 'Pamplona, España', 'ADMIN'),
                        ('cliente21', 'password21', 'Antonio', 'Jiménez', '+34 789012346', 'San Sebastián, España', 'ADMIN'),
                        ('cliente22', 'password22','Marta','Navarro','+34 890123457','Burgos, España','USER'),
                        ('cliente23', 'password23','Pedro','Díaz','+34 901234568','Salamanca, España','USER'),
                        ('cliente24', 'password24','Teresa' ,'Santos' ,'+34 012345679' ,'Toledo ,España' ,'MECANICO'),
                        ('cliente25', 'password25' ,'Fernando' ,'Domínguez' ,'+34 123456781' ,'León ,España ','USER'),
                        ('cliente26', 'password26 ','Pilar ','Vázquez ','+34 234567802 ','Lleida ,España ','USER'),
                        ('cliente27', 'password27', 'Carmen', 'Gutiérrez', '+34 678901235', 'Pamplona, España', 'ADMIN'),
                        ('cliente28', 'password28', 'Antonio', 'Jiménez', '+34 789012346', 'San Sebastián, España', 'MECANICO'),
                        ('cliente29', 'password29','Marta','Navarro','+34 890123457','Burgos, España','MECANICO'),
                        ('cliente30', 'password30','Pedro','Díaz','+34 901234568','Salamanca, España','USER'),
                        ('cliente31', 'password31', 'Carmen', 'Gutiérrez', '+34 678901235', 'Pamplona, España', 'ADMIN'),
                        ('cliente32', 'password32', 'Antonio', 'Jiménez', '+34 789012346', 'San Sebastián, España', 'ADMIN'),
                        ('cliente33', 'password33','Marta','Navarro','+34 890123457','Burgos, España','USER'),
                        ('cliente34', 'password34','Pedro','Díaz','+34 901234568','Salamanca, España','USER'),
                        ('cliente35', 'password35','Teresa','Santos' ,'+34 012345679' ,'Toledo ,España' ,'USER'),
                        ('cliente36', 'password36' ,'Fernando' ,'Domínguez' ,'+34 123456781' ,'León ,España ','USER'),
                        ('cliente37', 'password37' ,'Pilar ','Vázquez ','+34 234567802 ','Lleida ,España ','USER'),
                        ('cliente38', 'password38', 'Antonio', 'Jiménez', '+34 789012346', 'San Sebastián, España', 'ADMIN'),
                        ('cliente39', 'password39','Marta','Navarro','+34 890123457','Burgos, España','USER'),
                        ('cliente40', 'password40','Pedro','Díaz','+34 901234568','Salamanca, España','USER'),
                        ('cliente41', 'password41' ,'Teresa' ,'Santos' ,'+34 012345679' ,'Toledo ,España' ,'USER'),
                        ('cliente42', 'password42' ,'Fernando' ,'Domínguez' ,'+34 123456781' ,'León ,España ','MECANICO'),
                        ('cliente43', 'password43 ','Pilar ','Vázquez ','+34 234567802 ','Lleida ,España ','MECANICO')
                    ;
                ";
                    MySqlCommand command = new MySqlCommand(insertCliente, this.connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Datos de prueba insertados en la tabla Cliente.");
                }

                // Verificar si ya existen registros en la tabla Vehiculo.
                selectCliente = "SELECT COUNT(*) FROM Vehiculo";
                selectCommand = new MySqlCommand(selectCliente, this.connection);
                result = selectCommand.ExecuteScalar();
                count = Convert.ToInt32(result);

                // Si no se encuentran registros en la tabla Vehiculo, insertar datos de prueba
                if (count == 0)
                {

                    // Insertar datos de prueba en la tabla Vehiculo
                    string insertVehiculo = @"
                    INSERT INTO Vehiculo (matricula, tipo, modelo, marca, clienteId)
                    VALUES
                        ('AAA1111','Coche','Civic','Honda',(SELECT id FROM Cliente WHERE username='cliente1')),
                        ('BBB2222','Moto','Ninja','Kawasaki',(SELECT id FROM Cliente WHERE username='cliente1')),
                        ('CCC3333','Camión','T680','Kenworth',(SELECT id FROM Cliente WHERE username='cliente2')),
                        ('DDD4444','Tractor','8R Series','John Deere',(SELECT id FROM Cliente WHERE username='cliente3')),
                        ('EEE5555','Ciclomotor','Vespa Primavera 125','Piaggio',(SELECT id FROM Cliente WHERE username='cliente3')),
                        ('FFF6666', 'Furgoneta', 'Sprinter', 'Mercedes-Benz', (SELECT id FROM Cliente WHERE username='cliente3')),
                        ('GGG7777','Coche','Accord','Honda',(SELECT id FROM Cliente WHERE username='cliente1')),
                        ('HHH8888','Moto','CBR600RR','Honda',(SELECT id FROM Cliente WHERE username='cliente1')),
                        ('III9999','Camión','W900','Kenworth',(SELECT id FROM Cliente WHERE username='cliente2')),
                        ('JJJ0000','Tractor','9R Series','John Deere',(SELECT id FROM Cliente WHERE username='cliente3')),
                        ('KKK1111','Ciclomotor', 'Liberty 125', 'Piaggio', (SELECT id FROM Cliente WHERE username='cliente3')),
                        ('LLL2222', 'Furgoneta', 'Transit', 'Ford', (SELECT id FROM Cliente WHERE username='cliente3')),
                        ('MMM3333', 'Coche', 'Model S', 'Tesla', (SELECT id FROM Cliente WHERE username='cliente4')),
                        ('NNN4444', 'Moto', 'R1', 'Yamaha', (SELECT id FROM Cliente WHERE username='cliente4')),
                        ('OOO5555', 'Camión', 'Lonestar', 'International Trucks',(SELECT id FROM Cliente WHERE username='cliente5')),
                        ('PPP6666' , 'Tractor' , 'Magnum Series' , 	'Case IH' ,( SELECT id FROM Cliente WHERE username = 'cliente5')),
                        ('QQQ7777','Coche','Camry','Toyota',(SELECT id FROM Cliente WHERE username='cliente1')),
                        ('RRR8888','Moto','GSX-R1000','Suzuki',(SELECT id FROM Cliente WHERE username='cliente1')),
                        ('SSS9999','Camión','VNL','Volvo',(SELECT id FROM Cliente WHERE username='cliente2')),
                        ('TTT0000','Tractor','T8 Series', 'New Holland', (SELECT id FROM Cliente WHERE username='cliente3')),
                        ('UUU1111', 'Ciclomotor', 'NMAX 125', 'Yamaha', (SELECT id FROM Cliente WHERE username='cliente3')),
                        ('VVV2222', 'Furgoneta', 'Ducato', 'Fiat Professional', (SELECT id FROM Cliente WHERE username='cliente3')),
                        ('WWW3333', 'Coche', 'Model X', 'Tesla',(SELECT id FROM Cliente WHERE username='cliente4')),
                        ('XXX4444' , 'Moto' , 'MT-07' , 'Yamaha' ,( SELECT id FROM Cliente WHERE username = 'cliente4')),
                        ('YYY5555' , 'Camión' , 'Cascadia' , 'Freightliner Trucks',( SELECT id FROM Cliente WHERE username = 'cliente5')),
                        ('ZZZ6666' , 'Tractor' , 'Steiger Series' ,'Case IH',( SELECT id FROM Cliente WHERE username = 'cliente5')),
                        ('AAB7777','Coche','Corolla','Toyota',(SELECT id FROM Cliente WHERE username='cliente6')),
                        ('ABB8888','Moto','YZF-R6','Yamaha',(SELECT id FROM Cliente WHERE username='cliente6')),
                        ('ABC9999','Camión','Lonestar International Trucks', 'Scania', (SELECT id FROM Cliente WHERE username='cliente7')),
                        ('ABD0000','Tractor', 'Axion Series ', 'Claas',(SELECT id FROM Cliente WHERE username='cliente7'))
                    ; 
                ";

                    MySqlCommand command = new MySqlCommand(insertVehiculo, this.connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Datos de prueba insertados en la tabla Vehiculo.");
                }

                // Verificar si ya existen registros en la tabla Pieza.
                selectCliente = "SELECT COUNT(*) FROM Pieza";
                selectCommand = new MySqlCommand(selectCliente, this.connection);
                result = selectCommand.ExecuteScalar();
                count = Convert.ToInt32(result);

                // Si no se encuentran registros en la tabla Pieza, insertar datos de prueba
                if (count == 0)
                {
                    // Insertar datos de prueba en la tabla Pieza
                    string insertPieza = @"
                    INSERT INTO Pieza (nombre, descripcion, stock, precio)
                    VALUES ('Bujía', 'Descripción de la bujía', 10, 5.99),
                            ('Motor', 'Descripción del motor', 10, 500.00),
                            ('Llantas', 'Descripción de las llantas', 40, 75.00),
                            ('Frenos', 'Descripción de los frenos', 40, 100.00),
                            ('Batería', 'Descripción de la batería', 20 ,50.00),
                            ('Radiador', 'Descripción del radiador',10 ,80.00),
                            ('Alternador','Descripción del alternador' ,10 ,120.00),
                            ('Filtro de aire','Descripción del filtro de aire' ,30 ,15.99),
                            ('Correa de distribución','Descripción de la correa de distribución' ,20 ,45.99),
                            ('Embrague','Descripción del embrague' ,10 ,150.00),
                            ('Amortiguadores','Descripción de los amortiguadores' ,40 ,200.00),
                            ('Bomba de agua', 'Descripción de la bomba de agua', 10 ,30.00),
                            ('Bomba de aceite', 'Descripción de la bomba de aceite', 10 ,40.00),
                            ('Caja de cambios', 'Descripción de la caja de cambios', 10 ,300.00),
                            ('Catalizador','Descripción del catalizador' ,10 ,250.00),
                            ('Compresor del aire acondicionado','Descripción del compresor del aire acondicionado' ,10 ,150.00),
                            ('Diferencial','Descripción del diferencial' ,10 ,200.00),
                            ('Escape','Descripción del escape' ,10 ,100.00)
                    ;
                ";

                    MySqlCommand command = new MySqlCommand(insertPieza, this.connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Datos de prueba insertados en la tabla Pieza.");
                }

                /*

                selectCliente = "SELECT COUNT(*) FROM Reparacion";
                selectCommand = new MySqlCommand(selectCliente, this.connection);
                result = selectCommand.ExecuteScalar();
                count = Convert.ToInt32(result);

                // Si no se encuentran registros en la tabla Reparacion, insertar datos de prueba
                if (count == 0)
                {
                    // Insertar datos de prueba en la tabla Reparacion
                    string insertReparacion = @"
                    INSERT INTO Reparacion (VehiculoId, Horas, PrecioPorHora, PrecioSinIva,Iva,DiaInicioReparacion,MecanicoId ,Finalizado)
                    VALUES (1 ,10.5 ,50.0 ,525.0 ,21.0 ,'2022-01-01',1,false),
                           (2 ,8.0 ,40.0 ,320.0 ,21.0 ,'2022-02-15',2,true),
                           (3 ,12.5 ,60.0 ,750.0 ,21.0 ,'2022-03-20',3,false)
                    ;
                ";

                    MySqlCommand command = new MySqlCommand(insertReparacion, this.connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Datos de prueba insertados en la tabla Reparacion.");
                }
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
