using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetsWebsite.Extensions;
using PetsWebsite.Models;
using PetsWebsite.Models.ViewModels;

namespace PetsWebsite.Controllers.API
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class ProductManageAPIController : ControllerBase
    {
        private readonly PetsDBContext _petsDB;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GoogleMapService _googleMapService;

        public ProductManageAPIController(PetsDBContext petsDB, IWebHostEnvironment webHostEnvironment, GoogleMapService googleMapService)
        {
            _petsDB = petsDB;
            _webHostEnvironment = webHostEnvironment;
            _googleMapService = googleMapService;
        }
        public List<Product> GetProduct()
        {
            var query = _petsDB.Products.Where(p => p.CompanyId == User.GetId()).OrderBy(p => p.State).Select(p => p).ToList();
            return query;
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
        [Route("{keyword?}")]
        public List<Product> SearchProductByKeyword(string? keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _petsDB.Products.Where(p => p.CompanyId == User.GetId()).OrderBy(p => p.State).ToList();
            }
            else
            {
                string trimedKeyword = keyword.Trim();
                return _petsDB.Products.Where(p => p.ProductName.Contains(trimedKeyword)).ToList();
            }
        }

        //廠商管理取得診所資料
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
                Emergency = c.Emergency == true ? "是" : "否"
            }).ToList();
        }
        //新增診所
        [HttpPost]
        public bool NewClinic([FromForm] CreateClinicViewModel Ownerclinic)
        {
            // 取出圖片及名稱
            var InputFile = HttpContext.Request.Form.Files[0];
            var InputFilePath = "";
            string PhotoPath;
            if (InputFile == null)
            {
                PhotoPath = "clinic_Default";
            }
            else
            {
                // 儲存photo
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                PhotoPath = $"{User.GetId()}_{UniqueId}.{PhotoFormat}";

                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Clinic", PhotoPath);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyTo(fs);
                fs.Close();
            }

            // 存取地址轉經緯度
            var result = _googleMapService.GetLatLngByAddr($"{Ownerclinic.City}{Ownerclinic.Region}{Ownerclinic.Address}");

            Clinic newClinic = new Clinic()
            {
                CompanyId = User.GetId(),
                ClinicName = Ownerclinic.ClinicName,
                Phone = Ownerclinic.Phone,
                City = Ownerclinic.City,
                Region = Ownerclinic.Region,
                Address = Ownerclinic.Address,
                Describe = Ownerclinic.Describe,
                Emergency = Ownerclinic.Emergency == "true" ? true : false,
                PhotoPath = PhotoPath,
                Latitude = Convert.ToDouble(result.lat),
                Longitude = Convert.ToDouble(result.lng),
                State = false
            };
            try
            {
                _petsDB.Clinics.Add(newClinic);
                _petsDB.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        //編輯診所頁面顯示資料
        [HttpGet]
        [Route("{EditClinicId}")]
        public bool EditClickData(int EditClinicId)
        {
            try
            {
                var EditClinic = _petsDB.Clinics.Where(c => c.ClinicId == EditClinicId && c.CompanyId == User.GetId()).Select(c => new EditClinicDataViewModels()
                {
                    ClinicId = c.ClinicId,
                    ClinicName = c.ClinicName,
                    Phone = c.Phone,
                    City = c.City,
                    Region = c.Region,
                    Address = c.Address,
                    Describe = c.Describe,
                    Emergency = c.Emergency,
                    PhotoPath = c.PhotoPath,
                    Service = c.Service,
                    ClinicMap = c.ClinicMap
                }).FirstOrDefault();
                HttpContext.Session.SetString("EditClinicData", JsonConvert.SerializeObject(EditClinic));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //取得編輯診所資料
        [HttpGet]
        public EditClinicDataViewModels GetEditClickData()
        {
            return JsonConvert.DeserializeObject<EditClinicDataViewModels>(HttpContext.Session.GetString("EditClinicData"));
        }
        //編輯診所
        [HttpPost]
        public bool EditClinic([FromForm] AfterEditClinicViewModel AfterEditClinic)
        {
            var EditNewClinic = _petsDB.Clinics.FirstOrDefault(c => c.ClinicId == AfterEditClinic.ClinicId);
            if (HttpContext.Request.Form.Files.Count != 0)
            {
                var oldPhoto = EditNewClinic.PhotoPath;
                if (!string.IsNullOrEmpty(oldPhoto))
                {
                    var oldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Clinic", oldPhoto);
                    System.IO.File.Delete(oldPhotoPath);
                }
                // 取出圖片及名稱
                var InputFile = HttpContext.Request.Form.Files[0];
                var InputFilePath = InputFile.FileName;
                string PhotoPath;
                // 儲存photo
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                EditNewClinic.PhotoPath = $"{User.GetId()}_{UniqueId}.{PhotoFormat}";
                InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Clinic", EditNewClinic.PhotoPath);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile.CopyTo(fs);
                fs.Close();
            }
            // 存取地址轉經緯度
            var result = _googleMapService.GetLatLngByAddr($"{AfterEditClinic.City}{AfterEditClinic.Region}{AfterEditClinic.Address}");

            EditNewClinic.ClinicName = AfterEditClinic.ClinicName;
            EditNewClinic.Phone = AfterEditClinic.Phone;
            EditNewClinic.City = AfterEditClinic.City;
            EditNewClinic.Region = AfterEditClinic.Region;
            EditNewClinic.Address = AfterEditClinic.Address;
            EditNewClinic.Describe = AfterEditClinic.Describe;
            EditNewClinic.Service = AfterEditClinic.Service;
            EditNewClinic.ClinicMap = AfterEditClinic.ClinicMap;
            EditNewClinic.Latitude = Convert.ToDouble(result.lat);
            EditNewClinic.Longitude = Convert.ToDouble(result.lng);
            EditNewClinic.Emergency = AfterEditClinic.Emergency == "true" ? true : false;
            try
            {
                _petsDB.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        //刪除診所
        [HttpDelete]
        [Route("{DeleteClinicId}")]
        public bool DeleteClinic(int DeleteClinicId)
        {
            var DeleteClinic = _petsDB.Clinics.FirstOrDefault(c => c.ClinicId == DeleteClinicId);
            try
            {
                string PhotoPath = DeleteClinic.PhotoPath;
                _petsDB.Remove(DeleteClinic);
                _petsDB.SaveChanges();
                if (!string.IsNullOrEmpty(PhotoPath))
                {
                    var oldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Clinic", PhotoPath);
                    System.IO.File.Delete(oldPhotoPath);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        // 獲取上架失敗產品
        [HttpGet]
        public List<Product> GetPublishFailProducts()
        {
            return _petsDB.Products.Where(p => p.CompanyId == User.GetId() && p.State == false && p.AuditResult != null).Select(p => p).ToList();
        }

        // 獲取上架失敗餐廳
        [HttpGet]
        public List<Restaurant> GetPublishFailRestaurants()
        {
            return _petsDB.Restaurants.Where(r => r.CompanyId == User.GetId() && r.State == false && r.AuditResult != null).Select(r => r).ToList();
        }

        // 獲取上架失敗診所
        [HttpGet]
        public List<Clinic> GetPublishFailClinics()
        {
            return _petsDB.Clinics.Where(c => c.CompanyId == User.GetId() && c.State == false && c.AuditResult != null).Select(c => c).ToList();
        }
    }
}
