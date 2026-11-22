
using Microsoft.Data.Sqlite;

class Program
{
    static void Main()
    {
        // 1. Establecer la conexion
        string connectionString = "Data Source=videojuegos.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            Console.WriteLine("Conexion establecida correctamente.");

            // 2. Crear la tabla
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Videojuegos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL UNIQUE,
                        Genero TEXT NOT NULL,
                        Anio INTEGER NOT NULL
                    );";

                command.ExecuteNonQuery();
            }

            // 3. Insertar tres registros
            string[,] videojuegos =
            {
                { "Minecraft", "Sandbox", "2011" },
                { "The Sims 4", "Simulacion", "2014" },
                { "Stardew Valley", "RPG", "2016" }
            };

            foreach (var i in Enumerable.Range(0, 3))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT OR IGNORE INTO Videojuegos
                        (Nombre, Genero, Anio)
                        VALUES ($nombre, $genero, $anio);";

                    command.Parameters.AddWithValue(
                        "$nombre", videojuegos[i, 0]);

                    command.Parameters.AddWithValue(
                        "$genero", videojuegos[i, 1]);

                    command.Parameters.AddWithValue(
                        "$anio", int.Parse(videojuegos[i, 2]));

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Registros insertados correctamente.");

            // 4. Consultar los registros
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT Id, Nombre, Genero, Anio
                    FROM Videojuegos;";

                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("\n--- LISTA DE VIDEOJUEGOS ---");

                    while (reader.Read())
                    {
                        Console.WriteLine(
                            $"ID: {reader.GetInt32(0)}");

                        Console.WriteLine(
                            $"Nombre: {reader.GetString(1)}");

                        Console.WriteLine(
                            $"Genero: {reader.GetString(2)}");

                        Console.WriteLine(
                            $"Año: {reader.GetInt32(3)}");

                        Console.WriteLine("-------------------------");
                    }
                }
            }
        }

        Console.WriteLine("\nPrograma finalizado.");
    }
}

