using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MainPolicy",
          corsPolicyBuilder =>
          {
                corsPolicyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                //TODO: remove this line for production
                corsPolicyBuilder.SetIsOriginAllowed(x => true);
          });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("MainPolicy");

app.MapControllers();

app.Run();
