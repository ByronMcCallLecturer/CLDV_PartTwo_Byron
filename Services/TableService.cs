using Azure;
using Azure.Data.Tables;
using SemesterTwo.Models;
using System.Threading.Tasks;

namespace SemesterTwo.Services
{
    public class TableService
    {
        private readonly TableClient _tableClient;

        public TableService(IConfiguration configuration)
        {
            var serviceClient = new TableServiceClient(configuration["AzureStorage:ConnectionString"]);
            _tableClient = serviceClient.GetTableClient("CustomerProfiles");
            _tableClient.CreateIfNotExists();
        }

        public async Task AddEntityAsync(CustomerProfile profile)
        {
            await _tableClient.AddEntityAsync(profile);
        }
    }
}
