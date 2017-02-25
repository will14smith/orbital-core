using System;
using System.Data;
using System.Linq;

namespace Respawn
{

    internal static class Db
    {
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, params object[] parameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            return ReplaceWithParams(cmd, parameters);
        }

        // NOTE: Custom code - probably doesn't work with anything other than PostgeSQL
        private static IDbCommand ReplaceWithParams(IDbCommand command, object[] parameters)
        {
            var text = command.CommandText;
            var args = parameters.Select((_, i) => ":p" + i);
            command.CommandText = string.Format(text, args.Cast<object>().ToArray());

            for (var index = 0; index < parameters.Length; index++)
            {
                var param = parameters[index];
                var dbParam = command.CreateParameter();
                dbParam.ParameterName = "p" + index;
                dbParam.Value = param;
                dbParam.DbType = GetDbType(param);

                command.Parameters.Add(dbParam);
            }

            return command;
        }

        private static DbType GetDbType(object o)
        {
            if (o is string)
            {
                return DbType.String;
            }

            throw new NotImplementedException();
        }
    }
}