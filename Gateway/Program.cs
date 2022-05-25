using Gateway.Service;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
string txt = "";

// Add services to the container.
//cors service
builder.Services.AddCors(options =>
{
    options.AddPolicy(txt,
    builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.UseGrpcWeb(new GrpcWebOptions{ DefaultEnabled = true});
app.MapGrpcService<OrderService>().RequireCors(txt);
if (app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();
app.MapControllers();

app.Run();
