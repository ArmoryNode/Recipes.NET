namespace RecipesDotNet.Server.Infrastructure;

public static class Constants
{
    public static class Authentication
    {
        public static class ExternalProviders
        {
            public const string Google = nameof(Google);
            public const string Microsoft = nameof(Microsoft);
        }

        public static class Credentials
        {
            public const string ClientId = nameof(ClientId);
            public const string ClientSecret = nameof(ClientSecret);
        }
    }

    public static class Authorization
    {
        public static class Policies
        {
        }
    }

    public static class HttpClientNames
    {
        public const string MicrosoftGraph = "Microsoft Graph";
        public const string UserImageClient = "User Image";
    }
}
