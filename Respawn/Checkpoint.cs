﻿using System.Data;

namespace Respawn
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;

    public class Checkpoint
    {
        private string[] _tablesToDelete;
        private string _deleteSql;

        public string[] TablesToIgnore { get; set; } = new string[0];
        public string[] SchemasToInclude { get; set; } = new string[0];
        public string[] SchemasToExclude { get; set; } = new string[0];
        public IDbAdapter DbAdapter { get; set; } = Respawn.DbAdapter.SqlServer;

        public int? CommandTimeout { get; set; }

        private class Relationship
        {
            public string PrimaryKeyTable { get; set; }
            public string ForeignKeyTable { get; set; }

            public bool IsSelfReferencing => PrimaryKeyTable == ForeignKeyTable;

        }

        public virtual void Reset(IDbConnection connection)
        {
            if (string.IsNullOrWhiteSpace(_deleteSql))
            {
                BuildDeleteTables(connection);
            }

            ExecuteDeleteSql(connection);
        }

        private void ExecuteDeleteSql(IDbConnection connection)
        {
            using (var tx = connection.BeginTransaction())
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandTimeout = CommandTimeout ?? cmd.CommandTimeout;
                cmd.CommandText = _deleteSql;
                cmd.Transaction = tx;

                cmd.ExecuteNonQuery();

                tx.Commit();
            }
        }

        private void BuildDeleteTables(IDbConnection connection)
        {
            var allTables = GetAllTables(connection);

            var allRelationships = GetRelationships(connection);

            _tablesToDelete = BuildTableList(allTables, allRelationships);

            _deleteSql = DbAdapter.BuildDeleteCommandText(_tablesToDelete);
        }

        private static string[] BuildTableList(ICollection<string> allTables, IList<Relationship> allRelationships,
            List<string> tablesToDelete = null)
        {
            if (tablesToDelete == null)
            {
                tablesToDelete = new List<string>();
            }

            var referencedTables = allRelationships
                .Where(rel => !rel.IsSelfReferencing)
                .Select(rel => rel.PrimaryKeyTable)
                .Distinct()
                .ToList();

            var leafTables = allTables.Except(referencedTables).ToList();

            if (referencedTables.Count > 0 && leafTables.Count == 0)
            {
                string message = string.Join(",", referencedTables);
                message = string.Join(Environment.NewLine, $@"There is a dependency involving the DB tables ({message}) and we can't safely build the list of tables to delete.",
                    "Check for circular references.",
                    "If you have TablesToIgnore you also need to ignore the tables to which these have primary key relationships.");
                throw new InvalidOperationException(message);
            }

            tablesToDelete.AddRange(leafTables);

            if (referencedTables.Any())
            {
                var relationships = allRelationships.Where(x => !leafTables.Contains(x.ForeignKeyTable)).ToArray();
                var tables = allTables.Except(leafTables).ToArray();
                BuildTableList(tables, relationships, tablesToDelete);
            }

            return tablesToDelete.ToArray();
        }

        private IList<Relationship> GetRelationships(IDbConnection connection)
        {
            var rels = new List<Relationship>();
            var commandText = DbAdapter.BuildRelationshipCommandText(this);

            var values = new List<string>();
            values.AddRange(TablesToIgnore ?? Enumerable.Empty<string>());
            values.AddRange(SchemasToExclude ?? Enumerable.Empty<string>());
            values.AddRange(SchemasToInclude ?? Enumerable.Empty<string>());

            using (var cmd = connection.CreateCommand(commandText, values.ToArray()))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rel = new Relationship
                        {
                            PrimaryKeyTable = "\"" + reader.GetString(0) + "\".\"" + reader.GetString(1) + "\"",
                            ForeignKeyTable = "\"" + reader.GetString(2) + "\".\"" + reader.GetString(3) + "\""
                        };
                        rels.Add(rel);
                    }
                }
            }

            return rels;
        }

        private IList<string> GetAllTables(IDbConnection connection)
        {
            var tables = new List<string>();

            string commandText = DbAdapter.BuildTableCommandText(this);

            var values = new List<string>();
            values.AddRange(TablesToIgnore ?? Enumerable.Empty<string>());
            values.AddRange(SchemasToExclude ?? Enumerable.Empty<string>());
            values.AddRange(SchemasToInclude ?? Enumerable.Empty<string>());

            using (var cmd = connection.CreateCommand(commandText, values.ToArray()))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            tables.Add("\"" + reader.GetString(0) + "\".\"" + reader.GetString(1) + "\"");
                        }
                        else
                        {
                            tables.Add("\"" + reader.GetString(1) + "\"");
                        }
                    }
                }
            }

            return tables.ToList();
        }
    }
}