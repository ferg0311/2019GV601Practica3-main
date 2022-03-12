using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using equiposWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace equiposWebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly prestamosContext _context;

        public equiposController(prestamosContext miContexto)
        {
            this._context = miContexto;
        }

        ///<summary>
        ///Metodo para retornar todos los reg. de la tabla de EQUIPOS
        ///</summary>
        ///<returns></returns>
        [HttpGet]
        [Route("api/equipos")]
        public IActionResult Get()
        {
            IEnumerable<equipos> equiposList = (from e in _context.equipos select e);
            if (equiposList.Count() > 0)
            {
                return Ok(equiposList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/equipos/{idUsuarios}")]
        public IActionResult Get(int idUsuario)
        {
            equipos equipo = (from e in _context.equipos
                              where e.id_equipos == idUsuario
                              select e
                                                ).FirstOrDefault();

            if (equipo != null)
            {
                return Ok(equipo);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("api/equipos")]
        public IActionResult guardarEquipo([FromBody] equipos equipoNuevo)
        {
            try
            {
                _context.equipos.Add(equipoNuevo);
                _context.SaveChanges();
                return Ok(equipoNuevo);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/equipos")]
        public IActionResult updateEquipo([FromBody] equipos equipoAModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            equipos equipoExiste = (from e in _context.equipos
                                    where e.id_equipos == equipoAModificar.id_equipos
                                    select e).FirstOrDefault();
            if (equipoExiste is null)
            {
                // Si no existe el registro retornar un NO ENCONTRADO
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos a modificar
            equipoExiste.nombre = equipoAModificar.nombre;
            equipoExiste.descripcion = equipoAModificar.descripcion;

            //Se envia el objeto a la base de datos.
            _context.Entry(equipoExiste).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(equipoExiste);
        }

    }
}
