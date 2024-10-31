using System.Globalization;
using AutoMapper;
using CsvHelper;
using TestBenchmark;
using TestBenchmark.Entities;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main()
    {
        // System.Console.Write("Заполнить БД? [Y/N] : ");
        // var message = Console.ReadLine();
        // if (message?.ToLower() == "y")
        // {
        //     await TestDb.Create();
        // }

        // await TestDb.Create();
        var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

        // RequestsToDb requestsToDb = new RequestsToDb();

        // try
        // {
        //     await requestsToDb.RequestsWithLardgeDbNormal();
        // }
        // catch (Exception ex)
        // {
        //     System.Console.WriteLine(ex.Message);
        // }
    }
}

class PeopleClass
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Sex { get; set; }

    public string? Email { get; set; }

    public string? JobTitle { get; set; }
}

class ComplimentClass
{
    public string? Compliment { get; set; }
}

class OrganizationClass
{
    public string? Name { get; set; }

    public string? WebSite { get; set; }

    public string? Country { get; set; }

    public string? Description { get; set; }

    public string? Founded { get; set; }

    public string? Industry { get; set; }
}

static class TestDb
{
    public static async Task Create()
    {
        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MapperProfile>()));

        var patchOrganizations = "/home/dunice/Downloads/organizations-10000.csv";
        var patchPeoples = "/home/dunice/Downloads/people-100000.csv";
        var complimentsPeoples = "/home/dunice/Downloads/compliments-100.csv";

        List<PeopleClass> peoplesCSV;
        List<OrganizationClass> organizationsCSV;
        List<ComplimentClass> complimentsCSV;

        var entityToDb = new List<OrganizationEntity>();

        using (var reader = new StreamReader(complimentsPeoples))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                complimentsCSV = csv.GetRecords<ComplimentClass>().ToList();
            }
        }

        using (var reader = new StreamReader(patchPeoples))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                peoplesCSV = csv.GetRecords<PeopleClass>().ToList();
            }
        }

        using (var reader = new StreamReader(patchOrganizations))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                organizationsCSV = csv.GetRecords<OrganizationClass>().ToList();
            }
        }

        var peoplesEntity = mapper.Map<List<PeopleEntity>>(peoplesCSV);
        var organizationsEntity = mapper.Map<List<OrganizationEntity>>(organizationsCSV);
        var complimentsEntity = mapper.Map<List<ComplimentEntity>>(complimentsCSV);

        Parallel.For(0, peoplesEntity.Count - 1, (i) =>
        {
            peoplesEntity[i].Compliments = GetCompliments(complimentsEntity);
        });

        Parallel.For(0, organizationsEntity.Count - 1, (i) =>
        {
            organizationsEntity[i].Employees = peoplesEntity.Skip(i * 10).Take(10).ToList();
        });

        using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdb;Username=postgres;Password=1234"))
        {
            await dbContext.Organizations.AddRangeAsync(organizationsEntity);
            await dbContext.Peoples.AddRangeAsync(peoplesEntity);
            await dbContext.Compliments.AddRangeAsync(complimentsEntity);
            await dbContext.SaveChangesAsync();

            using (var dbContextSmall = new ApplicationDbContext("Host=localhost;Database=testdbsmall;Username=postgres;Password=1234"))
            {
                var data = await dbContext.Organizations
                .Include(x => x.Employees)!
                .ThenInclude(x => x.Compliments)
                .Take(1000)
                .ToListAsync();

                data.ForEach(x => x.Employees?.ForEach(x => x.Compliments?.ForEach(x => x.Peoples?.Clear())));

                await dbContextSmall.Organizations.AddRangeAsync(data);
                await dbContextSmall.SaveChangesAsync();
            }
        }
    }

    private static List<ComplimentEntity> GetCompliments(List<ComplimentEntity> compliments)
    {
        Random random = new Random();
        var result = new List<ComplimentEntity>();

        for (int i = 0; i < 2; i++)
        {
            result.Add(compliments[random.Next(0, compliments.Count)]);
        }

        return result;
    }
}