using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using VideoLibrary.Engine.Constants;
using VideoLibrary.Engine.Extensions;
using VideoLibrary.Engine.Models;
using VideoLibrary.Logger;

namespace VideoLibrary.Engine.Services
{
    public class MoviesService
    {
        private readonly IServiceApiClient _serviceApiClient;
        Dictionary<string, string> _headers = new Dictionary<string, string>();

        public MoviesService(IServiceApiClient serviceApiClient, string key)
        {
            _serviceApiClient = serviceApiClient;
            _headers.Add(APIHeaders.WebjetToken, key);
        }

        public async Task<IEnumerable<MovieDetail>> GetMoviesDetail(List<Movie> movies)
        {
            //Thread safe dictionary of movies
            ConcurrentDictionary<string, MovieDetail> moviesList = new ConcurrentDictionary<string, MovieDetail>();

            //Adding movies to dictionary if it's price is less
            ActionBlock<MovieDetail> movieListAction = new ActionBlock<MovieDetail>(item =>
            {
                if (item != null)
                {
                    var uniqueID = item.ID.Substring(2);
                    moviesList.AddOrUpdate(uniqueID, item, (n, m) =>
                    {
                        return item.Price < m.Price ? item : m;
                    });
                }
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 5 });

            //Getting movie details
            TransformBlock<Movie, MovieDetail> readAction = new TransformBlock<Movie, MovieDetail>(async (movie) =>
            {
                MovieDetail movieDetail;
                if (movie.ID.Substring(0, 2).Equals("cw"))
                {
                    movieDetail = await GetCinemaWorldMovie(movie.ID);
                }
                else
                {
                    movieDetail = await GetFilmWorldMovie(movie.ID);
                }
                return movieDetail;
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 5 });

            //sending all movies to transform block to fetch details and price
            foreach (var movie in movies)
            {
                await readAction.SendAsync<Movie>(movie);
            }

            //linking the output of transform block to action block
            readAction.LinkTo(movieListAction, new DataflowLinkOptions { PropagateCompletion = true });

            //waiting for process to complete without blocking the thread
            readAction.Complete();
            await movieListAction.Completion;

            return moviesList.Values.OrderBy(m => m.Title);
        }

        public async Task<List<Movie>> GetCinemaWorldMovies()
        {
            try
            {
                var client = _serviceApiClient.GetHttpClient(ExternalAPI.Webjet);
                var route = "api/cinemaworld/movies";
                var result = await client.SendSimpleAsync<MoviesResponse>(route, _headers);
                return result.Movies;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }

        public async Task<List<Movie>> GetFilmWorldMovies()
        {
            try
            {
                var client = _serviceApiClient.GetHttpClient(ExternalAPI.Webjet);
                var route = "api/filmworld/movies";
                var result = await client.SendSimpleAsync<MoviesResponse>(route, _headers);
                return result.Movies;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }

        public async Task<MovieDetail> GetCinemaWorldMovie(string id)
        {
            try
            {
                var client = _serviceApiClient.GetHttpClient(ExternalAPI.Webjet);
                var route = $"api/cinemaworld/movie/{id}";
                var result = await client.SendSimpleAsync<MovieDetail>(route, _headers);
                return result;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }

        public async Task<MovieDetail> GetFilmWorldMovie(string id)
        {
            try
            {
                var client = _serviceApiClient.GetHttpClient(ExternalAPI.Webjet);
                var route = $"api/filmworld/movie/{id}";
                var result = await client.SendSimpleAsync<MovieDetail>(route, _headers);
                return result;
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
                return null;
            }
        }
    }
}
