using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FinanSist.CrossCutting;
using WMSLite.WebApi;
using FinanSist.WebApi.Middleware;
using Serilog;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.



builder.Services.AddControllers();
builder.Services.AddControllersWithViews(options =>
{
    options.AllowEmptyInputInBodyModelBinding = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

# region 
Environment.SetEnvironmentVariable("Connection_db", builder.Configuration["ConnectionStrings:Connection_db"]);
#endregion

#region Swagger
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FinanSist API",
        Description = "API do FinanSist, sistema de gerenciamento de finanças pessoais.",
        Contact = new OpenApiContact
        {
            Name = "Gabriel Silva Rodrigues Mota",
            Email = "gabriel.rodrigues.mota@outlook.com",
            Url = new Uri("https://www.linkedin.com/in/gabriel-rodrigues-mota/")
        }

    });

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
});
#endregion

#region  DI
ConfigureRepository.Config(builder.Services);
ConfigureService.Config(builder.Services);
builder.Services.AddHttpContextAccessor();
#endregion

#region  Auth
var key = Encoding.ASCII.GetBytes(Settings.Secret); //Pegando o bytes da nossa string (key)
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Defininfido esquema de auth padrão
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  //Definindo esquema desafio padrão
}).AddJwtBearer(jwt =>
                {
                    jwt.RequireHttpsMetadata = false; //Não precisa do HTTPS
                    jwt.SaveToken = false; //Não salvar o token
                    jwt.TokenValidationParameters = new TokenValidationParameters //Informando os parametros para efetuar a validação do token
                    {
                        ValidateIssuerSigningKey = true, //Definindo a validação da SegurityKey
                        IssuerSigningKey = new SymmetricSecurityKey(key), // Definindo a chave que vai ser usado para validação.
                        ValidateLifetime = true, // Definindo que deve validar o tempo de expiração do token
                        ValidateIssuer = false, // Definindo que não deve validar o Issuer
                        ValidateAudience = false, // Definindo que não deve validar o Audience
                        ClockSkew = TimeSpan.Zero //Definindo a "inclinação do relógio"
                    };
                    jwt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.ContainsKey("tk_Finansist"))
                            {
                                context.Token = context.Request.Cookies["tk_Finansist"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
#endregion

#region Serilog
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
#endregion

#region CORS
/*
string[] listaCors = { };
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (environment == "Development")
{
    listaCors = new string[] {
                "https://localhost:4200",
                };
}
else
{
    listaCors = new string[] {
        "https://finansist.com.br"
    };
};
*/
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsApi", builder =>
    {
        builder.WithOrigins()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
        options.RoutePrefix = "";
    });
}
else
{

    app.UseMiddleware<SwaggerAuthorizedMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
        options.RoutePrefix = "docs";
    });
}



app.UseStaticFiles();

#region Middleware
app.UseMiddleware<ErrorHandlerMiddleware>();
#endregion


app.UseHttpsRedirection();

#region Metrics-Prometheus
app.UseRouting();
app.UseHttpMetrics(options =>
{
    //options.ReduceStatusCodeCardinality();
});
#endregion

app.UseCors("CorsApi");

#region  Auth
app.UseAuthentication();
app.UseAuthorization();
#endregion

app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
    endpoints.MapControllers();
});


app.Run();
