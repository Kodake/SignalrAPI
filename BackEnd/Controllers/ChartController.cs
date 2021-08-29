using BackEnd.Models;
using BackEnd.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChartController : ControllerBase
    {
        private readonly IEmpleadoRepository _repository;
        public ChartController(IEmpleadoRepository repository)
        {
            _repository = repository;
        }

        // GET: Chart
        [HttpGet]
        public ChartViewModel Get()
        {
            List<EmpleadoViewModel> chartValues = _repository.GetChartValues();
            
            ChartViewModel chart = new ChartViewModel
            {
                TotalSalaries = chartValues.Sum(x => x.Salario),
                FemaleSalaries = chartValues.Where(f => f.Genero.Contains("Femenino")).Sum(s => s.Salario),
                MaleSalaries = chartValues.Where(f => f.Genero.Contains("Masculino")).Sum(s => s.Salario),
            };

            return chart;
        }
    }
}
