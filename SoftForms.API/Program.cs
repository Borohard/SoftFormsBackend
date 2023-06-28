using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SoftForms.API.Services;
using SoftForms.Data.Repositories;
using System.Text;
using System.Text.Json.Serialization;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "http://localhost:3001",
                                              "http://www.contoso.com", "http://192.168.1.187:3000")
                                                .AllowAnyHeader()
                                                  .AllowAnyMethod(); ;
                      });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "http://localhost:5267",
            ValidAudience = "http://localhost:3000",

            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes("qweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqweqwe")),

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton(new MongoClient("mongodb://localhost:27017"));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SurveyRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<AuthenticationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseDeveloperExceptionPage();
app.Run();
