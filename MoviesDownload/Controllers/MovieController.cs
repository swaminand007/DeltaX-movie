using MoviesDownload.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesDownload.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult AddMovie()
        {
            IMDB_Context context = new IMDB_Context();
            var actorslist = context.Actors.Select(x => new SelectListItem
            {
                Value = x.ActorId.ToString(),
                Text = x.Name
            }).ToList();
            var producerlist = context.Producers.Select(x => new SelectListItem
            {
                Value = x.ProducerId.ToString(),
                Text = x.Name
            }).ToList();

            MovieModel model = new MovieModel()
            {
                Actors = actorslist,
                Producers = producerlist
            };
            return View("AddMovie", model);
        }
        [HttpPost]
        public ActionResult AddMovie(MovieModel objMovie, HttpPostedFileBase Image)
        {
            IMDB_Context context = new IMDB_Context();
            if (Image != null && Image.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Image.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                Image.SaveAs(path);

                objMovie.Poster = new byte[Image.ContentLength];
                Image.InputStream.Read(objMovie.Poster, 0, Image.ContentLength);
            }
            
                Producers objProducer = context.Producers.Where(x => x.ProducerId == objMovie.ProducerId).SingleOrDefault<Producers>();
                List<Actors> _ActorsList = new List<Actors>();
                foreach (var item in objMovie.ActorsId)
                    _ActorsList.Add(context.Actors.Where(x => x.ActorId == item).SingleOrDefault());
                Movies movie = new Movies()
                {
                    Name = objMovie.Name,
                    Plot = objMovie.Plot,
                    Yearofrelease = Convert.ToDateTime(objMovie.Yearofrelease),
                    Poster = objMovie.Poster,
                    Producer = objProducer,
                    Actors = _ActorsList
                };
                context.Movies.Add(movie);
                context.Entry(movie).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
                return RedirectToAction("GetMovies", "Imdb", null);
            
        }

        [HttpGet]
        public ActionResult EditMovie(int id)
        {
            IMDB_Context context = new IMDB_Context();

            Movies movie = context.Movies.Where(m => m.MovieId == id).SingleOrDefault<Movies>();
            Producers prod = context.Producers.Where(p => p.ProducerId == movie.ProducerId).SingleOrDefault();
            movie.Producer = prod;
            var _ProducerList = context.Producers.Where(x => x.ProducerId != prod.ProducerId).ToList<Producers>();
            var _ActorsList = context.Actors.ToList<Actors>();

            var _SelectListOfProducers = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text=prod.Name,Value=prod.ProducerId.ToString(),Selected=true}
                };
            var _SelectedListOfActors = new List<SelectListItem>();
            foreach (var item in _ProducerList)
            {
                _SelectListOfProducers.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.ProducerId.ToString()
                });
            };
            foreach (var item in _ActorsList)
            {
                _SelectedListOfActors.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.ActorId.ToString()
                });
            };
            MovieModel model = new MovieModel()
            {
                Name = movie.Name,
                Yearofrelease = movie.Yearofrelease,
                Plot = movie.Plot,
                Poster = movie.Poster,
                Producers = _SelectListOfProducers,
                Actors = _SelectedListOfActors,
                MovieId = movie.MovieId
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EditMovie(MovieModel objMovie, HttpPostedFileBase Image)
        {
            IMDB_Context context = new IMDB_Context();
            
                Producers objProducer = context.Producers.Where(x => x.ProducerId == objMovie.ProducerId).SingleOrDefault<Producers>();
                List<Actors> _ActorsList = new List<Actors>();
                foreach (var item in objMovie.ActorsId)
                    _ActorsList.Add(context.Actors.Where(x => x.ActorId == item).SingleOrDefault());
                Movies movie = new Movies()
                {
                    Name = objMovie.Name,
                    Plot = objMovie.Plot,
                    Yearofrelease = Convert.ToDateTime(objMovie.Yearofrelease),
                    Poster = objMovie.Poster,
                    Producer = objProducer,
                    Actors = _ActorsList
                };
                var mov = context.Movies.Where(x => x.MovieId == objMovie.MovieId).SingleOrDefault();
                context.Movies.Remove(mov);
                context.SaveChanges();
                mov.Name = movie.Name;
                mov.Plot = movie.Plot;
                mov.ProducerId = objProducer.ProducerId;
                mov.Actors = _ActorsList;
                mov.Yearofrelease = movie.Yearofrelease;
                context.Entry(mov).State = EntityState.Added;

                context.Movies.Add(mov);
                context.SaveChanges();
                return RedirectToAction("GetMovies", "Imdb", null);
            
        }
    }
}