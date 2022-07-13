using GoogleApi;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.AutoComplete.Request.Enums;
using Microsoft.AspNetCore.Mvc;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;

namespace WebClient.Controllers
{
    public class MapController : Controller
    {
        public IActionResult GetAutoPlace()
        {
            var x = new ForwardGeocoder();

            var r = x.Geocode(new ForwardGeocodeRequest
            {
                queryString = "1600 Pennsylvania Avenue, Washington, DC",

                BreakdownAddressElements = true,
                ShowExtraTags = true,
                ShowAlternativeNames = true,
                ShowGeoJSON = true
            });
            
            return Ok(r);
        }
    }
}
