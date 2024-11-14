using System.Text.Json.Serialization;
using learn.Helpers;
using learn.Repositories;
using learn.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddCors();
    services.AddControllers().AddJsonOptions(options =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.Configure<DBSetting>(builder.Configuration.GetSection("DBSetting"));

    services.AddSingleton<DataContext>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserService, UserService>();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}


var app = builder.Build();

//configure database and table
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.init();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandler>();

    app.MapControllers();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
