using AutoMapper;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using TestBenchmark.Entities;

namespace TestBenchmark
{
    [MemoryDiagnoser]
    public class RequestsToDb
    {
        // Смапить всех сотрудников в EmployeesResultDto  компании 0023cb5a-57c0-4689-9a77-cea7623f6899
        public readonly IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                   cfg.AddProfile<MapperProfile>()));

        [Benchmark]
        public void RequestsWithLardgeDbBad()
        {
            List<OrganizationEntity> org;
            using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdb;Username=postgres;Password=1234"))
            {
                org = dbContext.Organizations
                .Include(x => x.Employees)!
                .ThenInclude(x => x.Compliments)
                .ToList();
            }

            var emp = mapper.Map<List<EmployeesResultDto>>(org.FirstOrDefault(x => x.Id.ToString() == "148e779e-8982-4f44-8682-c04472c752dd")!.Employees!.ToList());

            foreach (var e in emp)
            {
                System.Console.WriteLine($"{e.FirstName} {e.LastName} {e.ComplimentsCount}");
            }
        }

        [Benchmark]
        public async Task RequestsWithLardgeDbNotWorst()
        {
            List<PeopleEntity> emp;
            using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdb;Username=postgres;Password=1234"))
            {
                var data = await dbContext.Organizations
                .Include(x => x.Employees)!
                .ThenInclude(x => x.Compliments)
                .FirstOrDefaultAsync(x => x.Id.ToString() == "148e779e-8982-4f44-8682-c04472c752dd");

                emp = data!.Employees!;
            }

            foreach (var e in mapper.Map<List<EmployeesResultDto>>(emp))
            {
                System.Console.WriteLine($"{e.FirstName} {e.LastName} {e.ComplimentsCount}");
            }
        }

        [Benchmark]
        public async Task RequestsWithLardgeDbNormalAsNoTracking()
        {
            List<EmployeesResultDto> emp;
            using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdb;Username=postgres;Password=1234"))
            {
                emp = await dbContext.Organizations
                .Where(x => x.Id.ToString() == "148e779e-8982-4f44-8682-c04472c752dd")
                .Include(x => x.Employees)!
                    .ThenInclude(x => x.Compliments)
                .SelectMany(x => x.Employees!, (org, emp) => new EmployeesResultDto(emp.FirstName, emp.LastName, emp.Compliments!.Count))
                .AsNoTracking()
                .ToListAsync();
            }

            Parallel.ForEach(emp, e =>
            {
                System.Console.WriteLine($"{e.FirstName} {e.LastName} {e.ComplimentsCount}");
            });
        }

        [Benchmark]
        public async Task RequestsWithLardgeDbNormalTracking()
        {
            List<EmployeesResultDto> emp;
            using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdb;Username=postgres;Password=1234"))
            {
                emp = await dbContext.Organizations
                .Where(x => x.Id.ToString() == "148e779e-8982-4f44-8682-c04472c752dd")
                .Include(x => x.Employees)!
                    .ThenInclude(x => x.Compliments)
                .SelectMany(x => x.Employees!, (org, emp) => new EmployeesResultDto(emp.FirstName, emp.LastName, emp.Compliments!.Count))
                .ToListAsync();
            }

            Parallel.ForEach(emp, e =>
            {
                System.Console.WriteLine($"{e.FirstName} {e.LastName} {e.ComplimentsCount}");
            });
        }

        [Benchmark]
        public void RequestsWithLardgeDbBadSmall()
        {
            List<OrganizationEntity> org;
            using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdbsmall;Username=postgres;Password=1234"))
            {
                org = dbContext.Organizations
                .Include(x => x.Employees)!
                .ThenInclude(x => x.Compliments)
                .ToList();
            }

            var emp = mapper.Map<List<EmployeesResultDto>>(org.FirstOrDefault(x => x.Id.ToString() == "148e779e-8982-4f44-8682-c04472c752dd")!.Employees!.ToList());

            foreach (var e in emp)
            {
                System.Console.WriteLine($"{e.FirstName} {e.LastName} {e.ComplimentsCount}");
            }
        }

        [Benchmark]
        public async Task RequestsWithLardgeDbNotWorstSmall()
        {
            List<PeopleEntity> emp;
            using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdbsmall;Username=postgres;Password=1234"))
            {
                var data = await dbContext.Organizations
                .Include(x => x.Employees)!
                .ThenInclude(x => x.Compliments)
                .FirstOrDefaultAsync(x => x.Id.ToString() == "148e779e-8982-4f44-8682-c04472c752dd");

                emp = data!.Employees!;
            }

            foreach (var e in mapper.Map<List<EmployeesResultDto>>(emp))
            {
                System.Console.WriteLine($"{e.FirstName} {e.LastName} {e.ComplimentsCount}");
            }
        }

        [Benchmark]
        public async Task RequestsWithLardgeDbNormalAsNoTrackingSmall()
        {
            List<EmployeesResultDto> emp;
            using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdbsmall;Username=postgres;Password=1234"))
            {
                emp = await dbContext.Organizations
                .Where(x => x.Id.ToString() == "148e779e-8982-4f44-8682-c04472c752dd")
                .Include(x => x.Employees)!
                    .ThenInclude(x => x.Compliments)
                .SelectMany(x => x.Employees!, (org, emp) => new EmployeesResultDto(emp.FirstName, emp.LastName, emp.Compliments!.Count))
                .AsNoTracking()
                .ToListAsync();
            }

            Parallel.ForEach(emp, e =>
            {
                System.Console.WriteLine($"{e.FirstName} {e.LastName} {e.ComplimentsCount}");
            });
        }

        [Benchmark]
        public async Task RequestsWithLardgeDbNormalTrackingSmall()
        {
            List<EmployeesResultDto> emp;
            using (var dbContext = new ApplicationDbContext("Host=localhost;Database=testdbsmall;Username=postgres;Password=1234"))
            {
                emp = await dbContext.Organizations
                .Where(x => x.Id.ToString() == "148e779e-8982-4f44-8682-c04472c752dd")
                .Include(x => x.Employees)!
                    .ThenInclude(x => x.Compliments)
                .SelectMany(x => x.Employees!, (org, emp) => new EmployeesResultDto(emp.FirstName, emp.LastName, emp.Compliments!.Count))
                .ToListAsync();
            }

            Parallel.ForEach(emp, e =>
            {
                System.Console.WriteLine($"{e.FirstName} {e.LastName} {e.ComplimentsCount}");
            });
        }
    }

    public record class EmployeesResultDto(string? FirstName, string? LastName, int ComplimentsCount);
}