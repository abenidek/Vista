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

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddCors(
    options => {
        options.AddDefaultPolicy(policy => {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.Run();

