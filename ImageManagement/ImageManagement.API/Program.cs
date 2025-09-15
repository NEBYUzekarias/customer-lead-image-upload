using ImageManagement.Application;
using ImageManagement.Persistence;
using ImageManagement.Domain;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
AddSwaggerDoc(builder.Services);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageManagement.Api v1"));
app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

// Initialize database with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BlogAppDbContext>();
    context.Database.EnsureCreated();
    
    // Add sample customers and leads if they don't exist
    if (!context.Customers.Any())
    {
        context.Customers.AddRange(
            new Customer { Id = 1, Name = "John Doe", Email = "john@example.com", Phone = "+1-555-0123" },
            new Customer { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Phone = "+1-555-0456" }
        );
    }
    
    if (!context.Leads.Any())
    {
        context.Leads.AddRange(
            new Lead { Id = 1, Name = "Alice Johnson", Email = "alice@company.com", Company = "Tech Corp" },
            new Lead { Id = 2, Name = "Bob Wilson", Email = "bob@business.com", Company = "Business Inc" }
        );
    }
    
    context.SaveChanges();
}

app.Run();

void AddSwaggerDoc(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Customer/Lead Image Management API",
            Description = "A comprehensive API for managing customer and lead profile images with a 10-image limit per entity",
            Contact = new OpenApiContact
            {
                Name = "Image Management API",
                Email = "support@imagemanagement.com"
            }
        });

        // Enable XML comments for better API documentation
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }

        // Configure Swagger to show detailed information
        c.DescribeAllParametersInCamelCase();
    });
}
