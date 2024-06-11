using Amazon.DynamoDBv2;
using DynamoDbCrudApi.Configurations;
using DynamoDbCrudApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure DynamoDB
var dynamoDbConfig = builder.Configuration.GetSection("DynamoDbLocal").Get<DynamoDbConfig>();
var clientConfig = new AmazonDynamoDBConfig
{
    ServiceURL = dynamoDbConfig!.ServiceURL
};

builder.Services.AddSingleton<IAmazonDynamoDB>(sp => new AmazonDynamoDBClient(clientConfig));

// Add PersonRepository
builder.Services.AddSingleton<PersonRepository>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
