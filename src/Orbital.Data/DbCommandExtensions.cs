using System;
using System.Data;

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
    }
}
