using AutoSearch.Api.Search.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//AddSearchSettings
ISearchSettings? settings = builder.Configuration.GetSection("SearchSettings").Get<SearchSettings>();
builder.Services.AddSingleton(settings);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
