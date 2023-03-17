using Microsoft.AspNetCore.Mvc;
using MvcCorEFProcedures.Models;
using MvcCorEFProcedures.Repositories;

namespace MvcCorEFProcedures.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryHospital repo;

        public EmpleadosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        //LA POSICION LA PRIMERA VEZ NO LO RECIBIMOS
        //CON int? LE DECIMOS QUE ES OPCIONAL HACIENDO QUE LA PRIMERA VEZ NO SE RECIBA
        public async Task<IActionResult> Index(int? posicion)
        {
            if(posicion == null)
            {
                posicion = 1;
            }
            int numRegistros= this.repo.GetNumeroEmpleados();
            ViewData["REGISTROS"] = numRegistros;
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync(posicion.Value);
            return View(empleados);
        }
    }
}
