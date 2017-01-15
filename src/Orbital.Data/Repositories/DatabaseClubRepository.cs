using Orbital.Models.Repositories;
using System.Collections.Generic;
using Orbital.Models.Domain;
using Orbital.Data.Connections;
using System.Data;

namespace Orbital.Data.Repositories
{
    public class DatabaseClubRepository : IClubRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseClubRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Club> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {SelectFields} FROM Club";

                using (var reader = command.ExecuteReader())
                {
                    return reader.ReadAll(MapToClub);
                }
            }
        }

        public Club GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {SelectFields} FROM Club WHERE Id = @Id";
                command.Parameters.Add(command.CreateParameter("Id", id, DbType.Int32));

                using (var reader = command.ExecuteReader())
                {
                    return reader.ReadOne(MapToClub);
                }
            }
        }

        public Club Create(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO Club (Name) VALUES (@Name) RETURNING Id";

                command.Parameters.Add(command.CreateParameter("Name", club.Name, DbType.String));

                var insertId = (int)command.ExecuteScalar();
                return new Club(insertId, club.Name);
            }
        }

        public Club Update(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE Club SET Name = @Name WHERE Id = @Id";

                command.Parameters.Add(command.CreateParameter("Id", club.Id, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("Name", club.Name, DbType.String));

                command.ExecuteNonQuery();
                return club;
            }
        }

        public bool Delete(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Club WHERE Id = @Id";
                    command.Parameters.Add(command.CreateParameter("Id", club.Id, DbType.Int32));

                    var rowsChanged = command.ExecuteNonQuery();

                    return rowsChanged == 1;
                }
            }
        }

        private static readonly string SelectFields = "Id, Name";
        private Club MapToClub(IDataRecord record)
        {
            var id = record.GetValue<int>(0);
            var name = record.GetValue<string>(1);

            return new Club(id, name);
        }
    }
}
