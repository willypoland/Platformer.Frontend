using Api.Internal;


namespace Api
{
    public static class ApiFactory
    {
        // public static IApi CreateApiSync() => new ApiSync();

        public static IApi CreateApiAsync() => new ApiAsync();

        // public static IApi CreateApiGGPO() => new ApiGGPO();

        public static IGameState CreateGameState() => new GameStateBridge();
    }
}