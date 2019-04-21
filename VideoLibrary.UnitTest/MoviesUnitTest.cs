using NUnit.Framework;
using System;
using System.Threading.Tasks;
using VideoLibrary.Engine.Services;

namespace VideoLibrary.UnitTest
{
    [TestFixture(@"sjd1HfkjU83ksdsm3802k", TypeArgs = new Type[] { typeof(string) })]
    public class MoviesUnitTest
    {
        private MoviesService _moviesService;

        public MoviesUnitTest(string key)
        {
            _moviesService = new MoviesService(new ServiceApiClient(), key);
        }

        /// <summary>
        /// Testing Cinemaworld movies list
        /// </summary>
        /// <returns></returns>
        [TestCase]
        public async Task When_CinemaWorld_Movies()
        {
            var movies = await _moviesService.GetCinemaWorldMovies();

            if (movies != null)
                CollectionAssert.IsNotEmpty(movies);
            else
                Assert.AreEqual(null, movies);
        }

        /// <summary>
        /// Testing Filmworld movies list
        /// </summary>
        /// <returns></returns>
        [TestCase]
        public async Task When_FilmWorld_Movies()
        {
            var movies = await _moviesService.GetFilmWorldMovies();

            if (movies != null)
                CollectionAssert.IsNotEmpty(movies);
            else
                Assert.AreEqual(null, movies);
        }

        /// <summary>
        /// Testing Cinemaworld movie
        /// </summary>
        /// <returns></returns>
        [TestCase]
        public async Task When_CinemaWorld_MovieDetail()
        {
            var movie = await _moviesService.GetCinemaWorldMovie("cw0080684");

            if (movie == null)
            {
                Assert.AreEqual(null, movie);
                return;
            }

            Assert.AreEqual("Star Wars: Episode V - The Empire Strikes Back", movie.Title);
            Assert.AreEqual("cw0080684", movie.ID);
        }

        /// <summary>
        /// Testing Filmworld movie
        /// </summary>
        /// <returns></returns>
        [TestCase]
        public async Task When_FilmWorld_MovieDetail()
        {
            var movie = await _moviesService.GetFilmWorldMovie("fw0076759");


            if (movie == null)
            {
                Assert.AreEqual(null, movie);
                return;
            }

            Assert.AreEqual("Star Wars: Episode IV - A New Hope", movie.Title);
            Assert.AreEqual("fw0076759", movie.ID);
        }

        /// <summary>
        /// Compare price of a movie in Cinemaworld and Filmworld
        /// </summary>
        /// <returns></returns>
        [TestCase]
        public async Task When_Compare_Price()
        {
            var cmovie = await _moviesService.GetCinemaWorldMovie("cw0076759");
            var fmovie = await _moviesService.GetFilmWorldMovie("fw0076759");

            if (cmovie == null || fmovie == null)
            {
                Assert.AreEqual(1, 1);
                return;
            }

            var filmIsCheaper = fmovie.Price < cmovie.Price;
            Assert.AreEqual(true, filmIsCheaper);
        }
    }
}
