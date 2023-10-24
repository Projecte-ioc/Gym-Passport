using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Npgsql;

namespace Gym_Passport.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            //_connectionString = "Host=gympassportser.postgres.database.azure.com;Username=mic_ad;Password=Meri120290!;Database=gympassportdb";
            // La base de datos fue eliminada así que volví al Docker que tengo en local para seguir avanzando.
            _connectionString = "Host=127.0.0.1;Username=mi_usuario;Password=mi_contraseña;Database=mi_basededatos";
        }

        protected NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
