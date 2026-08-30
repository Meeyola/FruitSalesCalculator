using FruitSalesCalculator.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FruitSalesCalculator.Repositories
{
    public class JsonFruitRepository : IFruitRepository
    {
        private readonly string _jsonFilePath;
   
        public JsonFruitRepository(string jsonFilePath)
        {
            if (string.IsNullOrWhiteSpace(jsonFilePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(jsonFilePath));

            _jsonFilePath = jsonFilePath;
        }
        public IReadOnlyList<FreshProduce> GetAll()
        {
            if (!File.Exists(_jsonFilePath))
                throw new FileNotFoundException($"Fruit data file not found at '{_jsonFilePath}'.", _jsonFilePath);

            string jsonContent;
            try
            {
                jsonContent = File.ReadAllText(_jsonFilePath);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                throw new InvalidOperationException($"Failed to read fruit data file at '{_jsonFilePath}'.", ex);
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                return JsonSerializer.Deserialize<List<FreshProduce>>(jsonContent, options) ?? new List<FreshProduce>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to deserialize fruit data from '{_jsonFilePath}'.", ex);
            }
        }
    }
}
