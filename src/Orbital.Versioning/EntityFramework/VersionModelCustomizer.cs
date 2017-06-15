using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace Orbital.Versioning
{
    internal class VersionModelCustomizer : ModelCustomizer
    {
        public static readonly string ModelMappingAnnotation = "VersionModelMapping";

        public override void Customize(ModelBuilder modelBuilder, DbContext dbContext)
        {
            base.Customize(modelBuilder, dbContext);

            var models = BuildModels(modelBuilder);

            foreach (var modelKvp in models)
            {
                var model = modelKvp.Value;

                var entityModel = model.EntityModel;
                var entityType = entityModel.ClrType;

                modelBuilder.Entity(model.VersionType, entityBuilder =>
                {
                    var originalTableName = entityModel.FindAnnotation("Relational:TableName")?.Value ?? entityType.Name;
                    entityBuilder.HasAnnotation("Relational:TableName", originalTableName + "_history");
                });
            }

            modelBuilder.Model.AddAnnotation(ModelMappingAnnotation, models);
        }

        private static IReadOnlyDictionary<string, VersionModel> BuildModels(ModelBuilder modelBuilder)
        {
            var builder = new VersionAssemblyBuilder();

            var models = modelBuilder.Model.GetEntityTypes()
                .ToDictionary(
                    model => model.ClrType.FullName,
                    model => builder.Add(model));

            var assembly = builder.Build();

            foreach (var modelKvp in models)
            {
                modelKvp.Value.Assembly = assembly;
            }

            return models;
        }
    }
}