namespace TestBenchmark.Entities
{
    public class ComplimentEntity
    {
        public long Id { get; set; }

        public string? Compliment { get; set; }

        public List<PeopleEntity>? Peoples { get; set; }
    }
}