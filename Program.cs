using System.Text.Json;
using ApiMovie.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

// Membuat builder untuk mengkonfigurasi aplikasi web
var builder = WebApplication.CreateBuilder(args);

// Konfigurasi routing agar URL otomatis lowercase (misalnya /Movie jadi /movie)
builder.Services.AddRouting(options => options.LowercaseUrls = true); // Add lowercase URLs

// Konfigurasi DbContext


// Menambahkan layanan autentikasi JWT menggunakan konfigurasi dari appsettings.json
builder.Services.AddJwtAuthentication(builder.Configuration); // Add JWT authentication

// Menambahkan layanan otorisasi
builder.Services.AddAuthorization(); // Add authorization

// Registrasi service untuk membuat JWT token, menggunakan Dependency Injection
builder.Services.AddScoped<JwtTokenGenerator>();

// Menambahkan controller ke dalam service container (untuk REST API)
builder.Services.AddControllers();

// builder.Services.AddApiVersioning(options =>
// {
//     options.ReportApiVersions = true; // Report API versions
//     options.DefaultApiVersion = new ApiVersion(1, 0); // Default API version
//     options.AssumeDefaultVersionWhenUnspecified = true; // Assume default version when unspecified
// });

// builder.Services.AddVersionedApiExplorer(options =>
// {
//     options.GroupNameFormat = "'v'VVV"; // Group name format
//     options.SubstituteApiVersionInUrl = true; // Substitute API version in URL
// });

// Menambahkan layanan endpoint API explorer (Swagger/OpenAPI)
builder.Services.AddEndpointsApiExplorer(); // Add Swagger


// Konfigurasi Swagger untuk dokumentasi API
builder.Services.AddSwaggerGen(options =>
{
    // Menambahkan definisi skema keamanan untuk JWT Bearer token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Masukkan token JWT Anda di sini (Bearer {token})",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });

    // Menentukan bahwa semua endpoint memerlukan skema keamanan Bearer
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});


// Membangun aplikasi web dari builder
var app = builder.Build();

// Konfigurasi pipeline HTTP
if (app.Environment.IsDevelopment())
{
    // Menampilkan Swagger UI hanya saat development
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

// Mengarahkan semua request HTTP ke HTTPS
app.UseHttpsRedirection();

// Middleware untuk menangani status code error (401 dan 403) dan memberi response JSON
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    
    if (response.StatusCode == 401)
    {
        response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(new
        {
            status = 401,
            message = "Unauthorized: Token is missing or invalid."
        });
        await response.WriteAsync(json);
    }
    else if (response.StatusCode == 403)
    {
        response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(new
        {
            status = 403,
            message = "Forbidden: You don't have permission to access this resource."
        });
        await response.WriteAsync(json);
    }
});

// Authentication & Authorization
// Middleware untuk autentikasi dan otorisasi pengguna
app.UseAuthentication();
app.UseAuthorization();

// Mendaftarkan semua controller sebagai endpoint API
app.MapControllers(); // Add controllers

app.Run();
