using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Orbital.Data
{
    public static class DbCommandExtensions
    {
        public static IDataParameter CreateParameter(this IDbCommand command, string name, object value, DbType type, ParameterDirection direction = ParameterDirection.Input)
        {
            var param = command.CreateParameter();

            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            param.DbType = type;
            param.Direction = direction;

            return param;
        }

        public static IDataParameter CreateParameter(this IDbCommand command, string name, object value, NpgsqlDbType type, ParameterDirection direction = ParameterDirection.Input)
        {
            var pgCommand = (NpgsqlCommand)command;
            var param = pgCommand.CreateParameter();

            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            param.NpgsqlDbType = type;
            param.Direction = direction;

            return param;
        }
    }
}
