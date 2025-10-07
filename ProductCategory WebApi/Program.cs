using Microsoft.EntityFrameworkCore;
using ProductCategory_WebApi.Application.Mapper;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Infrastructure.Datas;
using ProductCategory_WebApi.Infrastructure.Repositories;
using ProductCategory_WebApi.Infrastructure.Seeds;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategory, ProductCategoryRepository>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IGalleryRepository,GalleryRepository>();
builder.Services.AddScoped<ISizeRepository,SizeRepository>();
builder.Services.AddScoped<IProductColorRepository,ProductColorRepository>();
builder.Services.AddScoped<IProductGalleryRepository,ProductGalleryRepository>();
builder.Services.AddScoped<IProductSizeRepository,ProductSizeRepository>();


builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("product"),
        x => x.MigrationsAssembly("ProductCategoryWebApi.Infrastructure")
    )
);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DBContext>();
    context.Database.Migrate(); // Applies migrations
    await SeedData.InitializeAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
