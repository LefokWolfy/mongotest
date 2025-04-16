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
            // Force an index refresh to make the document searchable immediately.
            await _client.Indices.RefreshAsync("posts");
        }

        public async Task<ISearchResponse<Post>> SearchPostsAsync(string query)
        {
            // New approach: using QueryString targeting both Title and Text fields.
            // var response = await _client.SearchAsync<Post>(s => s
            //     .Query(q => q.QueryString(qs => qs
            //         .Fields(f => f.Field(p => p.Title).Field(p => p.Text))
            //         .Query(query)
            //     ))
            // );

            // Alternatively, you could use a MultiMatch query:
            
            var response = await _client.SearchAsync<Post>(s => s
                .Query(q => q.MultiMatch(mm => mm
                    .Fields(f => f
                        .Field(p => p.Title)
                        .Field(p => p.Text))
                    .Query(query)
                    .Fuzziness(Fuzziness.Auto)
                ))
            );
            

            return response;
        }
    }
}