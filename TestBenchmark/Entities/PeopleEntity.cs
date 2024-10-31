namespace TestBenchmark.Entities
{
    public class PeopleEntity
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Sex { get; set; }

        public string? Email { get; set; }

        public string? JobTitle { get; set; }

        public OrganizationEntity? Organization { get; set; }

        public Guid? OrganizationId { get; set; }

        public List<ComplimentEntity>? Compliments { get; set; }
    }
}