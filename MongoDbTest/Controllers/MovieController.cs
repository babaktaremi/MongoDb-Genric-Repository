using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbTest.GenericRepository.Repository;
using MongoDbTest.Model;

namespace MongoDbTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IRepository<Movie> _movieRepository;

        public MovieController(IRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            //var client = new MongoClient("mongodb://localhost:27017");
            //var db = client.GetDatabase("mymoviesdb");
          
            //var movies = db.GetCollection<Movie>("movies");
            //var movieList = await movies.Find(FilterDefinition<Movie>.Empty).ToListAsync();
            //await Task.CompletedTask;

            var d = await _movieRepository.GetCollection();

            return Ok(d);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Movie model)
        {
            await _movieRepository.Create(model);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Movie movie, string movieId)
        {
            
          var r= await _movieRepository.Update(movie, new ExpressionFilterDefinition<Movie>(m => m.Id == movieId));

          if (r)
              return Ok();

          return BadRequest();
        }


        [HttpGet]
        public async Task<IActionResult> GetMovieByYear(int year)
        {
            var result =
                await _movieRepository.GetWithFilter(
                    new ExpressionFilterDefinition<Movie>(movie => movie.Year == year));

            return Ok(result);
        }
    }
}
