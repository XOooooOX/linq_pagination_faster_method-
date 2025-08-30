var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/linq/{one:int}-{two:int}", (int one, int two) =>
    {
        var bigdata = Moq.Get(); 
        
        var paginationOld = Moq.GetOld(one, two);
        var paginationNew = Moq.GetNew(one, two);
        
        return Results.Ok(new {bigdata,paginationOld, paginationNew});
    })
    .WithName("linq")
    .WithOpenApi();

app.Run();

public class Moq
{
    public static List<Sample> Get()
        =>  Enumerable
        .Range(0, 100)
        .Select( o
            => new Sample(Id:o,Name:o.ToString()))
        .ToList();

    public static List<Sample> GetOld(int pageNumber, int pageSize)
        => Enumerable
            .Range(0, 100)
            .Select(o
                => new Sample(Id: o, Name: o.ToString()))
            .OrderBy(o => o.Id)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
        .ToList();

    public static List<Sample> GetNew(int lastId, int pageSize)
        => Enumerable
            .Range(0, 100)
            .Select(o
                => new Sample(Id: o, Name: o.ToString()))
            .OrderBy(o => o.Id)
            .Where(o => o.Id > lastId)
            .Take(pageSize)
            .ToList();
}

public record Sample(int Id,string Name);