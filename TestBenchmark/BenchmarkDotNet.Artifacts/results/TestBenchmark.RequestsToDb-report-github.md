```

BenchmarkDotNet v0.14.0, Ubuntu 22.04.5 LTS (Jammy Jellyfish)
Intel Core i5-10400 CPU 2.90GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.110
  [Host]     : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2


```
| Method                                      | Mean           | Error        | StdDev       | Gen0        | Gen1       | Gen2      | Allocated    |
|-------------------------------------------- |---------------:|-------------:|-------------:|------------:|-----------:|----------:|-------------:|
| RequestsWithLardgeDbBad                     | 6,061,090.7 μs | 70,921.32 μs | 66,339.85 μs | 117000.0000 | 42000.0000 | 3000.0000 | 765540.41 KB |
| RequestsWithLardgeDbNotWorst                |     1,139.9 μs |     22.72 μs |     31.84 μs |     23.4375 |     3.9063 |         - |    164.95 KB |
| RequestsWithLardgeDbNormalAsNoTracking      |     2,308.8 μs |     29.15 μs |     27.27 μs |      7.8125 |          - |         - |     64.67 KB |
| RequestsWithLardgeDbNormalTracking          |     2,293.0 μs |     13.62 μs |     12.07 μs |      7.8125 |          - |         - |     64.31 KB |
| RequestsWithLardgeDbBadSmall                |   354,887.4 μs |  7,050.94 μs | 15,770.44 μs |  12000.0000 |  5000.0000 | 1000.0000 |  77051.94 KB |
| RequestsWithLardgeDbNotWorstSmall           |     1,152.4 μs |     23.03 μs |     39.11 μs |     23.4375 |     3.9063 |         - |    164.95 KB |
| RequestsWithLardgeDbNormalAsNoTrackingSmall |       854.5 μs |     14.67 μs |     13.72 μs |      9.7656 |          - |         - |     64.93 KB |
| RequestsWithLardgeDbNormalTrackingSmall     |       849.0 μs |     11.14 μs |     10.42 μs |      9.7656 |          - |         - |     64.33 KB |
