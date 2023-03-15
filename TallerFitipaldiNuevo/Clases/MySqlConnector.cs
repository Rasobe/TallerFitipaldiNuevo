using MySql.Data.MySqlClient;
using System;

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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM Cliente WHERE username=@username AND password=@password";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return true;
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "INSERT INTO Cliente (username, password, nombre, apellidos, telefono, ubicacion) VALUES (@username, @password, @nombre, @apellidos, @telefono, @ubicacion)";
                cmd.Parameters.AddWithValue("@username", cliente.Username);
                cmd.Parameters.AddWithValue("@password", cliente.Password);
                cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
                cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
                cmd.Parameters.AddWithValue("@ubicacion", cliente.Ubicacion);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return true;

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

    }
}
