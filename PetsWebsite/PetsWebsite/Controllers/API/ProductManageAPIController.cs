using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModel;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers.API
{
    [Route("api/[controller]/{action}/{keyword?}")]
    [ApiController]
    public class ProductManageAPIController : ControllerBase
    {
        private readonly PetsDBContext _petsDB;

        public ProductManageAPIController(PetsDBContext petsDB)
        {
            _petsDB = petsDB;
        }
        public List<Product> GetProduct()
        {
            return _petsDB.Products.Where(p => p.CompanyId == User.GetId()).OrderBy(p => p.State).Select(p => p).ToList();
        }

        public async Task<List<Restaurant>> GetRestaurant()
        {
            int CompanyId = User.GetId();
            return await _petsDB.Restaurants.Where(r => r.CompanyId == CompanyId).Select(r => r).ToListAsync();
        }

        // 依產品單價排序asc
        public IEnumerable<Product> OrderByProductPriceAsc()
        {
            return _petsDB.Products.Where(p => p.CompanyId == User.GetId()).OrderBy(p => p.UnitPrice).ToList();
        }

        // 依產品單價排序desc
        public IEnumerable<Product> OrderByProductPriceDesc()
        {
            return _petsDB.Products.Where(p => p.CompanyId == User.GetId()).OrderByDescending(p => p.UnitPrice).ToList();
        }

        // 依產品庫存排序asc
        public IEnumerable<Product> OrderByProductUnitsInStockAsc()
        {
            return _petsDB.Products.Where(p => p.CompanyId == User.GetId()).OrderBy(p => p.UnitsInStock).ToList();
        }

        // 依產品庫存排序desc
        public IEnumerable<Product> OrderByProductUnitsInStockDesc()
        {
            return _petsDB.Products.Where(p => p.CompanyId == User.GetId()).OrderByDescending(p => p.UnitsInStock).ToList();
        }

        // 依關鍵字搜尋產品
        public List<Product> SearchProductByKeyword(string? keyword)
        {
            if(string.IsNullOrEmpty(keyword))
            {
                return _petsDB.Products.Where(p => p.CompanyId == User.GetId()).OrderBy(p => p.State).ToList();
            }
            else
            {
                string trimedKeyword = keyword.Trim();
                return _petsDB.Products.Where(p => p.ProductName.Contains(trimedKeyword)).ToList();
            }
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
