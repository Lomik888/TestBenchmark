namespace TestBenchmark.Entities
{
    public class OrganizationEntity
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? WebSite { get; set; }

        public string? Country { get; set; }

        public string? Description { get; set; }

        public string? Founded { get; set; }

        public string? Industry { get; set; }

        public List<PeopleEntity>? Employees { get; set; }
    }
}