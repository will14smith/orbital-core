namespace Orbital.Versioning
{
    public interface IVersionEntityWithMetadata<out TMetadata>
    {
        TMetadata ToMetadata();
    }
}