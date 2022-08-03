using Newtonsoft.Json;
using System.Net;

namespace PetsWebsite.Models
{
    public class GoogleMapService
    {
        string MapUrl = "https://maps.google.com/maps/api/geocode/json?region=tw&language=zh-TW&sensor=false&key=AIzaSyCrRxRAdFHWBd72yw26v0pIbeTOMWzVgw0";


        /// <summary>
        /// 傳入經緯度,取得住址
        /// </summary>
        /// <param name="Lat">緯度</param>
        /// <param name="Lng">經度</param>
        /// <returns>住址</returns>
        public string GetAddressByLatLng(string Lat, string Lng)
        {
            string strResult = "";
            GoogleGeoCodeResponse _mapdata = ConvertLatLngToAddress(Lat, Lng);
            if (_mapdata.status == "OK")
            {
                try
                {
                    strResult = _mapdata.results[0].formatted_address;
                }
                catch
                {

                }
            }
            return strResult;
        }
        /// <summary>
        /// 以住址查詢,回傳經緯度
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public location GetLatLngByAddr(string addr)
        {
            location _result = new location();
            GoogleGeoCodeResponse _mapdata = new GoogleGeoCodeResponse();
            _mapdata = ConvertAddressToLatLng(addr);
            if (_mapdata.status == "OK")
            {
                try
                {
                    _result.lat = _mapdata.results[0].geometry.location.lat;
                    _result.lng = _mapdata.results[0].geometry.location.lng;
                }
                catch
                {

                }
            }
            return _result;
        }
        /// <summary>
        /// 以經緯度呼叫Google Maps Api,回傳JsonResult 
        /// </summary>
        /// <param name="Lat">緯度</param>
        /// <param name="Lng">經度</param>
        /// <returns>經緯度</returns>
        public GoogleGeoCodeResponse ConvertLatLngToAddress(string Lat, string Lng)
        {
            string result = string.Empty;

            //string googlemapkey = "AIzaSyBGLwRnlbH6f3NYeXJFlxPT-FUdrHpv_O4";
            string url = MapUrl + "&latlng={0},{1}";
            url = string.Format(url, Lat, Lng);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            using (var response = request.GetResponse())
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {

                result = sr.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(result);
        }

        /// <summary>
        /// 以住址去取得Google Maps API Json results
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public GoogleGeoCodeResponse ConvertAddressToLatLng(string addr)
        {
            string result = string.Empty;

            //string googlemapkey = "AIzaSyB0HcesF2_f-WfmlqbX_vBXWXw-ZkNmgCE";
            string url = MapUrl + "&address={0}";
            url = string.Format(url, addr);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            using (var response = request.GetResponse())
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {

                result = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(result);
        }
        public class GoogleGeoCodeResponse
        {

            public string status { get; set; }
            public results[] results { get; set; }
        }
        public class results
        {
            public string formatted_address { get; set; }
            public geometry geometry { get; set; }
            public string[] types { get; set; }
            public address_component[] address_components { get; set; }
        }

        public class geometry
        {
            public string location_type { get; set; }
            public location location { get; set; }
        }

        public class location
        {
            public string lat { get; set; }
            public string lng { get; set; }
        }

        public class address_component
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public string[] types { get; set; }
        }
    }
}
