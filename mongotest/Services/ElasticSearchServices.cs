// filepath: c:\Users\Alvin\Documents\Uni\4 semester\PRJ4\mongotest\mongotest\Services\ElasticSearchService.cs
using Nest;
using mongoAPI.Models;
using Microsoft.Extensions.Configuration;

namespace mongoAPI.Services
{
    public class ElasticSearchService
    {
        private readonly ElasticClient _client;

        public ElasticSearchService(IConfiguration config)
        {
            // Get Elasticsearch URI from appsettings.json
            var elasticUri = config["Elasticsearch:Uri"];
            var settings = new ConnectionSettings(new Uri(elasticUri))
                .DefaultIndex("posts");
            _client = new ElasticClient(settings);
        }

        public async Task IndexPostAsync(Post post)
        {
            var response = await _client.IndexDocumentAsync(post);
            // Add error handling as needed.
        }

        public async Task<ISearchResponse<Post>> SearchPostsAsync(string query)
        {
            var response = await _client.SearchAsync<Post>(s => s
                .Query(q => q.Match(m => m
                    .Field(f => f.Title)
                    .Query(query)
                ))
            );
            return response;
        }
    }
}