namespace PinjamKelas.Api.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool IsDeleted => DeletedAt.HasValue;
    }
}