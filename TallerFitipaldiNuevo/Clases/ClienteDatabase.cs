using System.Collections.Generic;
using System.Data.SqlClient;

namespace TallerFitipaldiNuevo.Clases
{
    internal class ClienteDatabase
    {

        private readonly string connectionString;

        public ClienteDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InsertarCliente(Cliente cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Clientes (Username, Password, Nombre, Apellidos, Telefono, Ubicacion, Rol) " +
                               "VALUES (@Username, @Password, @Nombre, @Apellidos, @Telefono, @Ubicacion, @Rol)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Username", cliente.Username);
                command.Parameters.AddWithValue("@Password", cliente.Password);
                command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@Apellidos", cliente.Apellidos);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                command.Parameters.AddWithValue("@Ubicacion", cliente.Ubicacion);
                command.Parameters.AddWithValue("@Rol", cliente.Rol);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertarListaClientes(List<Cliente> listaClientes)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Clientes (Username, Password, Nombre, Apellidos, Telefono, Ubicacion, Rol) " +
                               "VALUES (@Username, @Password, @Nombre, @Apellidos, @Telefono, @Ubicacion, @Rol)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Username", "");
                command.Parameters.AddWithValue("@Password", "");
                command.Parameters.AddWithValue("@Nombre", "");
                command.Parameters.AddWithValue("@Apellidos", "");
                command.Parameters.AddWithValue("@Telefono", "");
                command.Parameters.AddWithValue("@Ubicacion", "");
                command.Parameters.AddWithValue("@Rol", "");

                connection.Open();

                foreach (Cliente cliente in listaClientes)
                {
                    command.Parameters["@Username"].Value = cliente.Username;
                    command.Parameters["@Password"].Value = cliente.Password;
                    command.Parameters["@Nombre"].Value = cliente.Nombre;
                    command.Parameters["@Apellidos"].Value = cliente.Apellidos;
                    command.Parameters["@Telefono"].Value = cliente.Telefono;
                    command.Parameters["@Ubicacion"].Value = cliente.Ubicacion;
                    command.Parameters["@Rol"].Value = cliente.Rol;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void BorrarClientePorId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Clientes WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void EditarClientePorId(int id, Cliente cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE clientes SET username=@username, password=@password, nombre=@nombre, apellidos=@apellidos, telefono=@telefono, ubicacion=@ubicacion, rol=@rol WHERE id=@id";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@username", cliente.Username);
                command.Parameters.AddWithValue("@password", cliente.Password);
                command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
                command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                command.Parameters.AddWithValue("@ubicacion", cliente.Ubicacion);
                command.Parameters.AddWithValue("@rol", cliente.Rol);
                command.ExecuteNonQuery();
            }
        }

        public Cliente SeleccionarClientePorUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clientes WHERE Username=@Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string password = (string)reader["Password"];
                        string nombre = (string)reader["Nombre"];
                        string apellidos = (string)reader["Apellidos"];
                        string telefono = (string)reader["Telefono"];
                        string ubicacion = (string)reader["Ubicacion"];
                        string rol = (string)reader["Rol"];

                        Cliente cliente = new Cliente(username, password, nombre, apellidos, telefono, ubicacion, rol);
                        cliente.Id = id;

                        return cliente;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public Cliente SeleccionarClientePorId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "SELECT * FROM Clientes WHERE Id=@Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string username = (string)reader["Username"];
                        string password = (string)reader["Password"];
                        string nombre = (string)reader["Nombre"];
                        string apellidos = (string)reader["Apellidos"];
                        string telefono = (string)reader["Telefono"];
                        string ubicacion = (string)reader["Ubicacion"];
                        string rol = (string)reader["Rol"];

                        Cliente cliente = new Cliente(username, password, nombre, apellidos, telefono, ubicacion, rol);
                        cliente.Id = id;

                        return cliente;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}

