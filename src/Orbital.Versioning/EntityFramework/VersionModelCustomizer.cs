using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Orbital.Versioning
{
    internal class VersionModelCustomizer : ModelCustomizer
    {
        private readonly IReadOnlyCollection<IVersionMetadataProvider> _metadata;
        private readonly VersionModelStore _versionModelStore;

        public VersionModelCustomizer(IReadOnlyCollection<IVersionMetadataProvider> metadata, VersionModelStore versionModelStore, ModelCustomizerDependencies dependencies)
            : base(dependencies)
        {           
            _metadata = metadata;
            _versionModelStore = versionModelStore;
        }

        public override void Customize(ModelBuilder modelBuilder, DbContext dbContext)
        {
            base.Customize(modelBuilder, dbContext);

            var models = BuildModels(modelBuilder);

            foreach (var modelKvp in models)
            {
                var efModel = modelKvp.Value.Item1;
                var model = modelKvp.Value.Item2;

                var entityModel = model.EntityModel;
                var entityType = entityModel.EntityType;

                modelBuilder.Entity(model.VersionType, entityBuilder =>
                {
                    var originalTableName = efModel.FindAnnotation("Relational:TableName")?.Value ?? entityType.Name;
                    entityBuilder.HasAnnotation("Relational:TableName", originalTableName + "_history");
                });
            }

            _versionModelStore.Models = models.ToDictionary(x => x.Key, x => x.Value.Item2);
        }

        private IReadOnlyDictionary<string, (IEntityType, VersionModel)> BuildModels(ModelBuilder modelBuilder)
        {
            var builder = new VersionAssemblyBuilder();

            var models = modelBuilder.Model.GetEntityTypes()
                .ToDictionary(
                    model => model.ClrType.FullName,
                    model => ((IEntityType)model, builder.Add(CreateEntityModel(model), _metadata)));

            var assembly = builder.Build();

            foreach (var modelKvp in models)
            {
                modelKvp.Value.Item2.Assembly = assembly;
            }

            return models;
        }

        private static EntityModel CreateEntityModel(IEntityType model)
        {
            return new EntityModel(
                entityType: model.ClrType,
                properties: model.GetProperties().Select(x => x.PropertyInfo).ToList());
        }
    }
}