using Microsoft.EntityFrameworkCore;
using Vista;
using Vista.Data.AppDbContext;
using Vista.Repository;
using Vista.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options => {
        options.SuppressAsyncSuffixInActionNames = false;
    });
builder.Services.AddDbContext<VistaDbContext>(
    opts => opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => 
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.Run();

