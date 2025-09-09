using Amazon.DynamoDBv2.DataModel;

namespace SuperAdminService.Models
{
    [DynamoDBTable("Users")]
    public class User
    {
        [DynamoDBHashKey]
        public int Id { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public string Email { get; set; }

        [DynamoDBProperty]
        public string Password { get; set; }

        [DynamoDBProperty]
        public string MobileNumber { get; set; }
    }

}
