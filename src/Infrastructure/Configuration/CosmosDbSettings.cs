namespace RecipesDotNet.Infrastructure.Configuration
{
    public readonly record struct CosmosDbSettings
    {
        public string DatabaseName { get; init; }
    }
}
