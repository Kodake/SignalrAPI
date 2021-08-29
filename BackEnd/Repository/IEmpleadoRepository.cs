using BackEnd.Models;
using System.Collections.Generic;

namespace BackEnd.Repository
{
    public interface IEmpleadoRepository
    {
        List<EmpleadoViewModel> GetChartValues();
    }
}
