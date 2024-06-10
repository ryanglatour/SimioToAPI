using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

public static class HttpClientFactoryProvider
{
    private static IServiceProvider _serviceProvider;

    static HttpClientFactoryProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public static IHttpClientFactory GetHttpClientFactory()
    {
        return _serviceProvider.GetService<IHttpClientFactory>();
    }
}
