namespace ClientDirectory.Domain.Common
{
    /// <summary>
    /// Base interface for entities in the domain.
    /// </summary>
    public interface IBase
    {
        /// <summary>
        /// Unique identifier for the entity.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Indicates if the entity is soft-deleted.
        /// </summary>
        public bool Deleted { get; set; }
    }
    /// <summary>
    /// Base class for domain entities, providing Id and Deleted properties.
    /// </summary>
    public class BaseEntity : IBase
    {
        /// <inheritdoc/>
        public string Id { get; set; }
        /// <inheritdoc/>
        public bool Deleted { get; set; }
    }
}
