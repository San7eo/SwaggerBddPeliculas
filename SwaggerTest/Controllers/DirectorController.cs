using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SwaggerTest.Domain.Entities;
using SwaggerTest.Domain.Request;
using SwaggerTest.Repository;

namespace SwaggerTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorController : ControllerBase
    {
        private readonly EJEMPLOCDASQLContext _context;
        public DirectorController( EJEMPLOCDASQLContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDirectores([FromQuery] GetDirectoresRequest request)
        {
            int skip = request.Skip;
            int take = request.Take;

            var result = _context.Directores.Skip(skip).Take(take).ToList();
            int count = _context.Directores.Count();

            return Ok( new {Datos = result, Count = count });
        }

        [HttpGet("{id}")]

        public IActionResult GetDirectorById([FromRoute] int id)
        {
            //var result = _context.Directores.Where(w => w.IdDirector == id).FirstOrDefault();
            var result = _context.Directores.FirstOrDefault(f => f.IdDirector == id);
            if (result == null) return NotFound(new { Error = $"No se encontro el id {id}" });
            return Ok(result);
            /*if (result != null) 
            {
                return Ok(result);
            }
            else
            {
                return NotFound(new { Error = $"No se encontro el id {id}" });
            }
            */
        }

        [HttpGet("peliculas")] //api/Director/peliculas

        public IActionResult GetPeliculasByDirector([FromQuery] int idDirector)
        {
            var result = _context.Directores.Where( w => w.IdDirector == idDirector )
                                            .Include( i => i.Peliculas )
                                            .ToList();
            return Ok( result );
        }

        // productoras sin pelicula
        [HttpGet("productorasPorId")]

        public IActionResult GetProductoresSinPeliculasById([FromQuery] int idProductor)
        {
            var result = _context.Productoras.Where(w => !w.Peliculas.Any() && w.IdProductora == idProductor)
                                             .FirstOrDefault();

            if (result == null) return NotFound($"No se encontro un productor sin peliculas con el id : {idProductor}");
            return Ok( result );
                                              
        }
        // actuaciones de una pelicula
        [HttpGet("actuaciones")]

        public IActionResult GetActuacionesDePelicula([FromQuery] int idPelicula)
        {
            var result = _context.Peliculas.Where(W => W.IdPelicula == idPelicula)
                                            .Include( i => i.Actuaciones)
                                            .ToList();
            if (result == null) return NotFound($"No se encontro un actuaciones de la pelicula con el id : {idPelicula}");
            return Ok(result);
        }

        // actuaciones de un actor
        [HttpGet("Actor_Actuaciones")]

        public IActionResult GetActuacionesDeActor([FromQuery] int idActor)
        {

            var result = _context.Actores
                         .Where(w => w.IdActor == idActor)
                         .SelectMany(a => a.Actuaciones
                             .Select(actuacion => new
                             {
                                 a.IdActor,
                                 NombreActor = a.Nombre,
                                 actuacion.IdActuacion,
                                 actuacion.Papel,
                                 TituloPelicula = actuacion.IdPeliculaNavigation.Titulo
                             })
                         )
                         .ToList();

            if (result == null || !result.Any())
                return NotFound($"No se encontraron actuaciones para el actor con el ID: {idActor}");

            return Ok(result);

        }
    }
}
