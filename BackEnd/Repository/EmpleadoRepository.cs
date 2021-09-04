using BackEnd.Hubs;
using BackEnd.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Repository
{
    public interface IDatabaseChangeNotificationService
    {
        void Config();
    }

    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly IHubContext<ChartHub> _context;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbctx;
        public EmpleadoRepository(IConfiguration configuration,
                                    IHubContext<ChartHub> context,
                                    ApplicationDbContext dbctx)
        {
            _configuration = configuration;
            _context = context;
            _dbctx = dbctx;
        }

        public void Config()
        {
            GetChartValues();
        }

        public List<EmpleadoViewModel> GetChartValues()
        {
            var chartValues = new List<EmpleadoViewModel>();
            string connString = _configuration.GetConnectionString("DevConnection");

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"SELECT Salario, Genero FROM [dbo].Empleado", conn))
                {
                    SqlDependency dependency = new SqlDependency(cmd);
                    dependency.OnChange += new OnChangeEventHandler(ChartValuesNotification);
                    SqlDependency.Start(connString);
                    var reader = cmd.ExecuteReader();

                    return GetValues(reader, chartValues);
                }
            }
        }

        private List<EmpleadoViewModel> GetValues(SqlDataReader reader, List<EmpleadoViewModel> chartValues)
        {
            while (reader.Read())
            {
                var chart = new EmpleadoViewModel
                {
                    Salario = Convert.ToDouble(reader["Salario"]),
                    Genero = Convert.ToString(reader["Genero"]),
                };

                chartValues.Add(chart);
            }
            return chartValues;
        }

        public ChartViewModel GetSalaries()
        {
            List<EmpleadoViewModel> chartValues = GetChartValues();

            ChartViewModel chart = new ChartViewModel
            {
                TotalSalaries = chartValues.Sum(x => x.Salario),
                FemaleSalaries = chartValues.Where(f => f.Genero.Contains("Femenino")).Sum(s => s.Salario),
                MaleSalaries = chartValues.Where(f => f.Genero.Contains("Masculino")).Sum(s => s.Salario),
            };

            return chart;
        }

        private void ChartValuesNotification(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                _context.Clients.All.SendAsync("getChartValues", GetSalaries());

                GetChartValues();
            }
        }
    }
}
