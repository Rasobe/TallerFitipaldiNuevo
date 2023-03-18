using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace TallerFitipaldiNuevo.Clases
{
    internal class MySqlConnector
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public bool IsConnected
        {
            get { return connection != null && connection.State == System.Data.ConnectionState.Open; }
        }

        public MySqlConnector(string server, string database, string uid, string password)
        {
            this.server = server;
            this.database = database;
            this.uid = uid;
            this.password = password;
        }

        public void Connect()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
            connection.Open();

        }

        public void Disconnect()
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }

        //
        //
        // LOGIN Y REGISTER
        //
        //

        public bool Login(string username, string password)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM cliente WHERE username=@username AND password=@password";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Disconnect();
                            return true;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error al iniciar sesión.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
            finally
            {
                Disconnect();
            }
            return false;
        }

        public bool Register(Cliente cliente)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO cliente (username, password, nombre, apellidos, telefono, ubicacion, rol) VALUES (@username,@password,@nombre,@apellidos,@telefono,@ubicacion,@rol)";
                    cmd.Parameters.AddWithValue("@username", cliente.Username);
                    cmd.Parameters.AddWithValue("@password", cliente.Password);
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@ubicacion", cliente.Ubicacion);
                    cmd.Parameters.AddWithValue("@rol", "USER");

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {

                        Disconnect();
                        return true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error al registrarse.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
            finally
            {
                Disconnect();
            }
            return false;
        }

        //
        //
        // CLIENTES
        //
        //

        public void InsertarCliente(Cliente cliente)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO cliente (username, password, nombre, apellidos, telefono, ubicacion, rol) VALUES (@username, @password, @nombre, @apellidos, @telefono,@ubicacion,@rol)";
                    cmd.Parameters.AddWithValue("@username", cliente.Username);
                    cmd.Parameters.AddWithValue("@password", cliente.Password);
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@ubicacion", cliente.Ubicacion);
                    cmd.Parameters.AddWithValue("@rol", cliente.Rol);

                    Connect();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Se ha añadido correctamente el cliente.", "Inserción exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Error al insertar cliente '" + cliente.ToString() + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
            finally
            {
                if (connection != null)
                {
                    Disconnect();
                }
            }
        }
        public void EliminarClientePorId(int id)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM cliente WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                MessageBox.Show("Error al eliminar el cliente con id '" + id + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
            finally
            {
                if (connection != null)
                {
                    Disconnect();
                }
            }
        }
        public void EditarClientePorId(int id, Cliente cliente)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE cliente SET username=@username, password=@password, nombre=@nombre, apellidos=@apellidos, telefono=@telefono, ubicacion=@ubicacion, rol=@rol WHERE id=@id";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@username", cliente.Username);
                    cmd.Parameters.AddWithValue("@password", cliente.Password);
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@ubicacion", cliente.Ubicacion);
                    cmd.Parameters.AddWithValue("@rol", cliente.Rol);

                    cmd.ExecuteNonQuery();
                    Disconnect();
                }
            }
            catch
            {
                MessageBox.Show("Error al editar cliente con id '" + id + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
            finally
            {
                if (connection != null)
                {
                    Disconnect();
                }
            }
        }

        public Cliente SeleccionarClientePorUsername(string username)
        {
            Connect();
            string query = "SELECT * FROM cliente WHERE username = @username";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Cliente cliente = new Cliente();
                        cliente.Id = reader.GetInt32("id");
                        cliente.Username = reader.GetString("username");
                        cliente.Password = reader.GetString("password");
                        cliente.Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString("nombre");
                        cliente.Apellidos = reader.IsDBNull(reader.GetOrdinal("apellidos")) ? null : reader.GetString("apellidos");
                        cliente.Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono");
                        cliente.Ubicacion = reader.IsDBNull(reader.GetOrdinal("ubicacion")) ? null : reader.GetString("ubicacion");
                        cliente.Rol = reader.IsDBNull(reader.GetOrdinal("rol")) ? null : reader.GetString("rol");
                        Disconnect();
                        return cliente;
                    }
                }
            }
            Disconnect();
            return null;
        }

        public bool existeClientePorUsername(string username)
        {
            Connect();
            string query = "SELECT * FROM cliente WHERE username = @username";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Disconnect();
                        return true;
                    }
                }
            }
            Disconnect();
            return false;
        }

        public Cliente SeleccionarClientePorId(int id)
        {
            Cliente cliente = null;
            {
                Connect();
                string query = "SELECT * FROM cliente WHERE id = @id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            Id = reader.GetInt32("id"),
                            Username = reader.GetString("username"),
                            Password = reader.GetString("password"),
                            Nombre = reader.GetString("nombre"),
                            Apellidos = reader.GetString("apellidos"),
                            Telefono = reader.GetString("telefono"),
                            Ubicacion = reader.GetString("ubicacion"),
                            Rol = reader.GetString("rol")
                        };
                    }
                }
            }
            Disconnect();
            return cliente;
        }

        public List<Cliente> SeleccionarTodosLosClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            Cliente cliente;
            {
                Connect();
                string query = "SELECT * FROM cliente";
                MySqlCommand command = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cliente =  new Cliente
                        {
                            Id = reader.GetInt32("id"),
                            Username = reader.GetString("username"),
                            Password = reader.GetString("password"),
                            Nombre = reader.GetString("nombre"),
                            Apellidos = reader.GetString("apellidos"),
                            Telefono = reader.GetString("telefono"),
                            Ubicacion = reader.GetString("ubicacion"),
                            Rol = reader.GetString("rol")
                        };
                        clientes.Add(cliente);
                    }
                }
            }
            Disconnect();
            return clientes;
        }

        //
        //
        // VEHÍCULOS
        //
        //
        public List<Vehiculo> SeleccionarTodosLosVehiculos()
        {
            Connect();
            string query = "SELECT * FROM vehiculo";
            List<Vehiculo> vehiculos = new List<Vehiculo>();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Vehiculo vehiculo = new Vehiculo();
                        vehiculo.Matricula = reader.GetString("matricula");
                        vehiculo.Tipo = reader.GetString("tipo");
                        vehiculo.Marca = reader.GetString("marca");
                        vehiculo.Modelo = reader.GetString("modelo");
                        vehiculo.ClienteId = reader.GetInt32("clienteId");
                        vehiculos.Add(vehiculo);
                    }
                }
            }
            Disconnect();
            return vehiculos;
        }

        public List<Vehiculo> SeleccionarTodosLosVehiculosDelCliente(int id)
        {
            Connect();
            string query = "SELECT * FROM vehiculo WHERE clienteId=@clienteId";
            List<Vehiculo> vehiculos = new List<Vehiculo>();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@clienteId", id);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Vehiculo vehiculo = new Vehiculo();
                        vehiculo.Matricula = reader.GetString("matricula");
                        vehiculo.Tipo = reader.GetString("tipo");
                        vehiculo.Marca = reader.GetString("marca");
                        vehiculo.Modelo = reader.GetString("modelo");
                        vehiculo.ClienteId = reader.GetInt32("clienteId");
                        vehiculos.Add(vehiculo);
                    }
                }
            }
            Disconnect();
            return vehiculos;
        }

        public void InsertarVehiculo(Vehiculo vehiculo)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO vehiculo (matricula, tipo, modelo, marca, clienteId) VALUES (@matricula, @tipo, @modelo, @marca, @clienteId)";
                    cmd.Parameters.AddWithValue("@matricula", vehiculo.Matricula);
                    cmd.Parameters.AddWithValue("@tipo", vehiculo.Tipo);
                    cmd.Parameters.AddWithValue("@modelo", vehiculo.Modelo);
                    cmd.Parameters.AddWithValue("@marca", vehiculo.Marca);
                    cmd.Parameters.AddWithValue("@clienteId", vehiculo.ClienteId);

                    Connect();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Se ha añadido correctamente el vehículo.", "Inserción exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Error al insertar vehículo '" + vehiculo.ToString() + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
            finally
            {
                if (connection != null)
                {
                    Disconnect();
                }
            }
        }

        public Vehiculo SeleccionarVehiculoPorMatricula(string matricula)
        {
            Connect();
            string query = "SELECT * FROM vehiculo WHERE matricula = @matricula";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@matricula", matricula);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Vehiculo vehiculo = new Vehiculo();
                        vehiculo.Id = reader.GetInt32("id");
                        vehiculo.Matricula = reader.GetString("matricula");
                        vehiculo.Tipo = reader.GetString("tipo");
                        vehiculo.Modelo = reader.GetString("modelo");
                        vehiculo.Marca = reader.GetString("marca");
                        vehiculo.ClienteId = reader.GetInt32("clienteId");
                        Disconnect();
                        return vehiculo;
                    }
                }
            }
            Disconnect();
            return null;
        }

        public Vehiculo SeleccionarVehiculoPorId(int id)
        {
            Connect();
            string query = "SELECT * FROM vehiculo WHERE id = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Vehiculo vehiculo = new Vehiculo();
                        vehiculo.Matricula = reader.GetString("matricula");
                        vehiculo.Tipo = reader.GetString("tipo");
                        vehiculo.Marca = reader.GetString("marca");
                        vehiculo.Modelo = reader.GetString("modelo");
                        vehiculo.ClienteId = reader.GetInt32("clienteId");
                        Disconnect();
                        return vehiculo;
                    }
                }
            }
            Disconnect();
            return null;
        }

        public void EliminarVehiculoPorMatricula(string matricula)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM vehiculo WHERE matricula = @matricula";
                    cmd.Parameters.AddWithValue("@matricula", matricula);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                MessageBox.Show("Error al eliminar el vehículo con matrícula '" + matricula + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
            finally
            {
                if (connection != null)
                {
                    Disconnect();
                }
            }
        }

        public void EditarVehiculoPorMatricula(String matricula, Vehiculo vehiculo)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE vehiculo SET matricula=@nuevaMatricula, tipo=@tipo, marca=@marca, modelo=@modelo, clienteId=@clienteId WHERE matricula=@viejaMatricula";

                    cmd.Parameters.AddWithValue("@nuevaMatricula", vehiculo.Matricula);
                    cmd.Parameters.AddWithValue("@viejaMatricula", matricula);
                    cmd.Parameters.AddWithValue("@tipo", vehiculo.Tipo);
                    cmd.Parameters.AddWithValue("@marca", vehiculo.Marca);
                    cmd.Parameters.AddWithValue("@modelo", vehiculo.Modelo);
                    cmd.Parameters.AddWithValue("@clienteId", vehiculo.ClienteId);

                    cmd.ExecuteNonQuery();
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                MessageBox.Show("Error al editar el vehiculo con matrícula '" + matricula + "'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnect();
            }
            finally
            {
                if (connection != null)
                {
                    Disconnect();
                }
            }
        }

        //
        //
        // PIEZAS
        //
        //

        public List<Pieza> SeleccionarTodasLasPiezas()
        {
            Connect();
            string query = "SELECT * FROM pieza";
            List<Pieza> piezas = new List<Pieza>();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pieza pieza = new Pieza();
                        pieza.Stock = reader.GetInt32("id");
                        pieza.Nombre = reader.GetString("nombre");
                        pieza.Descripcion = reader.GetString("descripcion");
                        pieza.Stock = reader.GetInt32("stock");
                        pieza.Precio = reader.GetFloat("precio");
                        piezas.Add(pieza);
                    }
                }
            }
            Disconnect();
            return piezas;
        }

        public int SeleccionarStockDePiezaPorNombre(string nombre)
        {
            Connect();
            string query = "SELECT * FROM pieza where nombre = @nombre";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", nombre);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32("stock");
                    }
                }
            }
            Disconnect();
            return -1;
        }

        public Pieza SeleccionarPiezaPorNombre(string nombre)
        {
            Connect();
            string query = "SELECT * FROM pieza WHERE nombre = @nombre";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", nombre);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Pieza pieza = new Pieza();
                        pieza.Stock = reader.GetInt32("id");
                        pieza.Nombre = reader.GetString("nombre");
                        pieza.Descripcion = reader.GetString("descripcion");
                        pieza.Stock = reader.GetInt32("stock");
                        pieza.Precio = reader.GetFloat("precio");
                        return pieza;
                    }
                }
            }
            Disconnect();
            return null;
        }

        //
        //
        // REPARACIONES
        //
        //

    }
}
