using Amazon.DynamoDBv2.DataModel;

namespace DynamoDbCrudApi.Models
{
    [DynamoDBTable("Persons")]
    public class Person
    {
        [DynamoDBHashKey]
        public string? Id { get; set; }

        [DynamoDBProperty]
        public string? Name { get; set; }

        [DynamoDBProperty]
        public int Age { get; set; }
    }
}
