using Orbital.Models.Repositories;
using System.Collections.Generic;
using Orbital.Models.Domain;
using Orbital.Data.Connections;
using System.Data;
using System;

namespace Orbital.Data.Repositories
{
    public class DatabasePersonRepository : IPersonRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabasePersonRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Person> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {SelectFields} FROM Person";

                using (var reader = command.ExecuteReader())
                {
                    return reader.ReadAll(MapToPerson);
                }
            }
        }

        public IReadOnlyCollection<Person> GetAllByClubId(int clubId)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {SelectFields} FROM Person WHERE ClubId = @ClubId";
                command.Parameters.Add(command.CreateParameter("ClubId", clubId, DbType.Int32));

                using (var reader = command.ExecuteReader())
                {
                    return reader.ReadAll(MapToPerson);
                }
            }
        }

        public Person GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {SelectFields} FROM Person WHERE Id = @Id";
                command.Parameters.Add(command.CreateParameter("Id", id, DbType.Int32));

                using (var reader = command.ExecuteReader())
                {
                    return reader.ReadOne(MapToPerson);
                }
            }
        }

        public Person Create(Person person)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Person (ClubId, Name, Gender, Bowstyle, ArcheryGBNumber, DateOfBirth, DateStartedArchery)" +
                    " VALUES (@ClubId, @Name, @Gender, @Bowstyle, @ArcheryGBNumber, @DateOfBirth, @DateStartedArchery) RETURNING Id";

                command.Parameters.Add(command.CreateParameter("ClubId", person.ClubId, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("Name", person.Name, DbType.String));
                command.Parameters.Add(command.CreateParameter("Gender", (int)person.Gender, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("Bowstyle", (int?)person.Bowstyle, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("ArcheryGBNumber", person.ArcheryGBNumber, DbType.String));
                command.Parameters.Add(command.CreateParameter("DateOfBirth", person.DateOfBirth, DbType.DateTime2));
                command.Parameters.Add(command.CreateParameter("DateStartedArchery", person.DateStartedArchery, DbType.DateTime2));

                var insertId = (int)command.ExecuteScalar();
                return new Person(insertId, person);
            }
        }

        public Person Update(Person person)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Person SET " +
                    " ClubId = @ClubId," +
                    " Name = @Name," +
                    " Gender = @Gender," +
                    " Bowstyle = @Bowstyle," +
                    " ArcheryGBNumber = @ArcheryGBNumber," +
                    " DateOfBirth = @DateOfBirth," +
                    " DateStartedArchery = @DateStartedArchery" +
                    " WHERE Id = @Id";

                command.Parameters.Add(command.CreateParameter("Id", person.Id, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("ClubId", person.ClubId, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("Name", person.Name, DbType.String));
                command.Parameters.Add(command.CreateParameter("Gender", (int)person.Gender, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("Bowstyle", (int?)person.Bowstyle, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("ArcheryGBNumber", person.ArcheryGBNumber, DbType.String));
                command.Parameters.Add(command.CreateParameter("DateOfBirth", person.DateOfBirth, DbType.DateTime2));
                command.Parameters.Add(command.CreateParameter("DateStartedArchery", person.DateStartedArchery, DbType.DateTime2));

                command.ExecuteNonQuery();
                return person;
            }
        }

        public bool Delete(Person person)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Person WHERE Id = @Id";
                command.Parameters.Add(command.CreateParameter("Id", person.Id, DbType.Int32));

                var rowsChanged = command.ExecuteNonQuery();

                return rowsChanged == 1;
            }
        }

        private static readonly string SelectFields = "Id, ClubId, Name, Gender, Bowstyle, ArcheryGBNumber, DateOfBirth, DateStartedArchery";
        private Person MapToPerson(IDataRecord record)
        {
            var id = record.GetValue<int>(0);
            var clubId = record.GetValue<int>(1);
            var name = record.GetValue<string>(2);
            var gender = record.GetValue<Gender>(3);
            var bowstyle = record.GetValue<Bowstyle?>(4);
            var archeryGBNumber = record.GetValue<string>(5);
            var dateOfBirth = record.GetValue<DateTime?>(6);
            var dateStartedArchery = record.GetValue<DateTime?>(7);

            return new Person(id, clubId, name, gender, bowstyle, archeryGBNumber, dateOfBirth, dateStartedArchery);

        }
    }
}
