using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModel;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers.API
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class ProductManageAPIController : ControllerBase
    {
        private readonly PetsDBContext _petsDB;

        public ProductManageAPIController(PetsDBContext petsDB)
        {
            _petsDB = petsDB;
        }
        public async Task<List<ProductViewModel>> GetProduct()
        {
            int CompanyId = 0;
            return await _petsDB.Products.Where(p => p.CompanyId == CompanyId).Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                PhotoPath = p.PhotoPath,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
                ProductDescription = p.Describe
            }).ToListAsync();
        }

        public async Task<List<Restaurant>> GetRestaurant()
        {
            int CompanyId = User.GetId();
            return await _petsDB.Restaurants.Where(r => r.CompanyId == CompanyId).Select(r => r).ToListAsync();
        }
        [HttpGet]
        public List<ClinicManageViewModel> GetOwnerClinic()
        {
            return _petsDB.Clinics.Where(c => c.CompanyId == User.GetId()).Select(c => new ClinicManageViewModel()
            {
                ClinicId = c.ClinicId,
                ClinicName = c.ClinicName,
                PhotoPath = c.PhotoPath,
                Phone = c.Phone,
                City = c.City,
                Region = c.Region,
                Address = c.Address,
                Describe = c.Describe,
                Emergency=c.Emergency==true?"是":"否"
            }).ToList();
        }
    }
}
