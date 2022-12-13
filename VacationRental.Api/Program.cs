using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using VacationRental.Api.Data;
using VacationRental.Api.Data.Interfaces;
using VacationRental.Api.Data.Repositories;
using VacationRental.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IRentalService, RentalService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

builder.Services.AddSingleton<IDataContext, DataContext>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
app.MapControllers();

app.Run();

public partial class Program { }