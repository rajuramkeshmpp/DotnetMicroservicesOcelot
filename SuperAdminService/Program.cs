using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Amazon;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var awsOptions = builder.Configuration.GetSection("AWS");
    var region = awsOptions["Region"];
    var accessKey = awsOptions["AccessKey"];
    var secretKey = awsOptions["SecretKey"];

    return new AmazonDynamoDBClient(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
});

builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();


builder.Services.AddAuthentication("JwtBearer")
    .AddJwtBearer("JwtBearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
