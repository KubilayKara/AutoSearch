using AutoSearch.Api.Search.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//AddSearchSettings
ISearchSettings? settings = builder.Configuration.GetSection("SearchSettings").Get<SearchSettings>();
builder.Services.AddSingleton(settings);

//CORS configuration for solve this error=> Access to XMLHttpRequest at 'https://localhost:7222/api/Search?text=test&num=100' from origin 'http://localhost:4200' has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource.
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowOrigin",
        builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }
    );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//CORS configuration
app.UseCors("AllowOrigin");

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
