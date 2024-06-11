using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynamoDbCrudApi.Models; // Asegúrate de que este espacio de nombres coincida
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDbCrudApi.Repositories // Asegúrate de que este espacio de nombres coincida
{
    public class PersonRepository
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly DynamoDBContext _context;

        public PersonRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
            _context = new DynamoDBContext(dynamoDbClient);
            InitializeTableAsync().Wait();
        }

        public async Task InitializeTableAsync()
        {
            var tableResponse = await _dynamoDbClient.ListTablesAsync();
            if (!tableResponse.TableNames.Contains("Persons"))
            {
                var request = new CreateTableRequest
                {
                    TableName = "Persons",
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement { AttributeName = "Id", KeyType = "HASH" }
                    },
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition { AttributeName = "Id", AttributeType = "S" }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                };

                await _dynamoDbClient.CreateTableAsync(request);
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Person>(conditions).GetRemainingAsync();
        }

        public async Task<Person> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<Person>(id);
        }

        public async Task SaveAsync(Person person)
        {
            await _context.SaveAsync(person);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.DeleteAsync<Person>(id);
        }
    }
}
