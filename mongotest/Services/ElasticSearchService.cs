// filepath: c:\Users\Alvin\Documents\Uni\4 semester\PRJ4\mongotest\mongotest\Services\ElasticSearchService.cs
using Nest;
using mongoAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace mongoAPI.Services
{
    public class ElasticSearchService
    {
        private readonly ElasticClient _client;

        public ElasticSearchService(IConfiguration config)
        {
            var elasticUri = config["Elasticsearch:Uri"];

            // Optionally, also move credentials to your appsettings.json
            var username = config["Elasticsearch:Username"];
            var password = config["Elasticsearch:Password"];
            
            var settings = new ConnectionSettings(new Uri(elasticUri))
                .DefaultIndex("posts")
                .BasicAuthentication(username, password)
                .ServerCertificateValidationCallback((o, cert, chain, errors) => true); // Bypass certificate validation (for dev only)

            _client = new ElasticClient(settings);
        }

        public async Task<IndexResponse> IndexPostAsync(Post post)
        {
            var response = await _client.IndexDocumentAsync(post);
            await _client.Indices.RefreshAsync("posts");
            return response;
        }
        
        public async Task<ISearchResponse<Post>> SearchPostsAsync(string query)
        {
            var response = await _client.SearchAsync<Post>(s => s
                .Query(q => q.MultiMatch(mm => mm
                    .Fields(f => f.Field(p => p.Title).Field(p => p.Text))
                    .Query(query)
                    .Fuzziness(Fuzziness.Auto)
                ))
            );
            return response;
        }
    }
}