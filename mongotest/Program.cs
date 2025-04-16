using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using mongoAPI.Services;
using mongotest.Services;
using mongotest;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<CommentService>();
builder.Services.AddSingleton<PostService>();
builder.Services.AddSingleton<ElasticSearchService>();


// Add services to the container
builder.Services.AddControllers();

// Configure Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MongoAPI", Version = "v1" });
});

var app = builder.Build();

// Seed default data
using (var scope = app.Services.CreateScope())
{
    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
    var postService = scope.ServiceProvider.GetRequiredService<PostService>();

    await SeedData.InitializeAsync(userService, postService);
}

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoAPI v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
