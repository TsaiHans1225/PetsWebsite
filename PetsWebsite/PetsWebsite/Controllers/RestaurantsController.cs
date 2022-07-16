using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Models;
using System.Data.SqlClient;

namespace PetsWebsite.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly PetsDBContext _dBContext;
        private readonly DbConfig _dbConfig;
        private readonly RestaurantService _restaurantService;
        public RestaurantsController(PetsDBContext dBContext, DbConfig dbConfig, RestaurantService restaurantService)
        {
            _dBContext = dBContext;
            _dbConfig = dbConfig;
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> Restaurant()
        {
            string _ConnectionString = _dbConfig.ConnectionString;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _ConnectionString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            string SqlCmd = "Select distinct City From Restaurants";
            cmd.CommandText = SqlCmd;
            cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> data = new List<string>();
            while (reader.Read())
            {
                data.Add(Convert.ToString(reader["City"]));
            }
            conn.Close();
            ViewBag.Data = data;
            ViewBag.QryByCity = _restaurantService.RestaurantQryByCity;
            return View("Restaurant");
        }
        [HttpGet]
        public IActionResult RestaurantDetails()
        {
            return View();
        }





    }
}
