using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Logic;
using PetsWebsite.Models;
using System.Data.SqlClient;

namespace PetsWebsite.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly PetsDBContext _dBContext;
        private readonly DbConfig _dbConfig;
        private readonly RestaurantService _restaurantService;
        private readonly IcommonLogic _commonLogic;

        public RestaurantsController(PetsDBContext dBContext, DbConfig dbConfig, RestaurantService restaurantService, IcommonLogic icommonLogic)
        {
            _dBContext = dBContext;
            _dbConfig = dbConfig;
            _restaurantService = restaurantService;
            _commonLogic = icommonLogic;
        }

        [HttpGet]
        public async Task<IActionResult> Restaurant()
        {
            //搜尋City
            string _ConnectionString = _dbConfig.ConnectionString;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _ConnectionString;
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            string Sql = "Select distinct City From Restaurants";
            cmd.CommandText = Sql;
            cmd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            List<string> data = new List<string>();
            while (reader.Read())
            {
                data.Add(Convert.ToString(reader["City"]));

            }
            conn.Close();
            //=============================================================
            //搜尋Region
            SqlConnection con = new SqlConnection();
            con.ConnectionString = _ConnectionString;
            con.Open();
            SqlCommand cd = con.CreateCommand();
            string SqlR = "Select distinct Region From Restaurants";
            cd.CommandText = SqlR;
            cd.CommandType = System.Data.CommandType.Text;
            SqlDataReader reader2 = cd.ExecuteReader();
            List<string> data2 = new List<string>();
            while (reader2.Read())
            {
                data2.Add(Convert.ToString(reader2["Region"]));
            }
            conn.Close();


            ViewBag.Data = data;
            ViewBag.QryByCity = _restaurantService.RestaurantQryByCity;
            ViewBag.region = data2;
            ViewBag.QryByRegion = _restaurantService.RestaurantQryByRegion;
            ViewBag.QryByCityRegion = _restaurantService.QryCityRegion;
            return View("Restaurant");
        }

        [HttpGet]
        public async Task<IActionResult> Restaurant2()
        {
            //搜尋City
            ViewBag.Data = _commonLogic.GetExistCity();
            //搜尋Region
            ViewBag.region = _commonLogic.GetExistegion();
            ViewBag.QryByCity = _restaurantService.RestaurantQryByCity;
            ViewBag.QryByRegion = _restaurantService.RestaurantQryByRegion;
            ViewBag.QryByCityRegion = _restaurantService.QryCityRegion;
            return View("Restaurant");
        }

        [HttpGet]

        public IActionResult RestaurantDetails()
        {
            return View();
        }





    }
}
