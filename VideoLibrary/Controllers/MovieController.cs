using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoLibrary.Engine.Models;
using VideoLibrary.Engine.Services;
using VideoLibrary.Logger;
using VideoLibrary.ResponseHelpers;

namespace VideoLibrary.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly MoviesService _moviesService;

        public MovieController(MoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        [HttpGet("movies")]
        public async Task<IActionResult> GetAllMovies()
        {
            try
            {
                //Fetching list from both APIs in parallel
                var cinemaworldMoviesTask = _moviesService.GetCinemaWorldMovies();
                var filmworldMoviesTask = _moviesService.GetFilmWorldMovies();

                await Task.WhenAll(cinemaworldMoviesTask, filmworldMoviesTask);

                var movies = cinemaworldMoviesTask.Result;

                if (movies != null)
                    movies.AddRange(filmworldMoviesTask.Result);
                else
                    movies = filmworldMoviesTask.Result;

                var result = await _moviesService.GetMoviesDetail(movies);

                return Ok(new CustomResponse<IEnumerable<MovieDetail>>
                {
                    Code = ResponseHelpers.StatusCode.Success,
                    Data = result
                });
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.Flatten().InnerExceptions)
                {
                    CommonLogger.LogError(e);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex);
            }

            return Ok(CustomExceptions.GenerateExceptionForApp("Error getting movies. Please reload list again."));
        }
    }
}
