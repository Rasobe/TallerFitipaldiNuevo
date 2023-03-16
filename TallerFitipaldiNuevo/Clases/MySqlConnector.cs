using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    cmd.CommandText = "INSERT INTO cliente (username, password, nombre, apellidos, telefono, ubicacion) VALUES (@username,@password,@nombre,@apellidos,@telefono,@ubicacion)";
                    cmd.Parameters.AddWithValue("@username", cliente.Username);
                    cmd.Parameters.AddWithValue("@password", cliente.Password);
                    cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@ubicacion", cliente.Ubicacion);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {

                        Disconnect();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
            return false;
        }

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
                }
            }
            catch (Exception ex)
            {
                // Maneja la excepción aqu
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
        public void BorrarClientePorId(int id)
        {
            try
            {
                Connect();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "DELETE FROM cliente WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    Connect();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Maneja la excepción aquí
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
            catch (Exception ex)
            {
                // Maneja la excepción aquí
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

    }
}
