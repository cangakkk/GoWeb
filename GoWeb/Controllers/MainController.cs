using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GoWeb.Models;
using GoWeb.Models;
using System.Text;

namespace GoWeb.Controllers
{
	public class MainController : Controller
	{
		private static readonly HttpClient httpClient = new HttpClient();
		private static string apiUrl;
		public MainController(IConfiguration _config)
		{
			apiUrl = _config["ApiUrl"];
		}

		// GET: MainController
		public async Task<IActionResult> Index()
		{
			VMResponse response = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "users"));
			if(response.data == null)
			{
				return View();
			}
			else
			{
				List<VMView> data = JsonConvert.DeserializeObject<List<VMView>>(response.data.ToString());
				return View(data);
			}
		}

        public async Task<IActionResult> Add()
        {
            ViewBag.Title = "Add Data";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(VMView dataView)
        {
            string jsonData = JsonConvert.SerializeObject(dataView);

            HttpContent content = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");

            VMResponse apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                                    await httpClient.PostAsync(apiUrl + "users", content)
                              ).Content.ReadAsStringAsync());

            if (apiResponse.status != "success")
            {
                string errorMag = apiResponse.message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int dataid)
        {
            VMResponse apiResponse = JsonConvert.DeserializeObject<VMResponse>(
                                    await httpClient.GetStringAsync(apiUrl + "users/" + dataid));

            VMView data = JsonConvert.DeserializeObject<VMView>(apiResponse.data.ToString());
            if (data == null || apiResponse.status != "success")
            {
                string errorMag = apiResponse.message;
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Delete Data";
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VMView dataView)
        {
            VMResponse apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                                    await httpClient.DeleteAsync(apiUrl + "users/" + dataView.Id)
                              ).Content.ReadAsStringAsync());

            if (apiResponse.status != "success")
            {
                string errorMag = apiResponse.message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int dataid)
        {
            VMResponse apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "users/" + dataid));
         

            if (apiResponse == null)
            {
                string errMsg = "No Response from API!";
                return RedirectToAction("Index");
            }

            VMView data = JsonConvert.DeserializeObject<VMView>(apiResponse.data.ToString());

            return View(data);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(VMView dataView)
        {
            string jsonData = JsonConvert.SerializeObject(dataView);
            HttpContent content = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
            VMResponse apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                                    await httpClient.PutAsync(apiUrl + "users/" + dataView.Id , content)
                              ).Content.ReadAsStringAsync());

            if (apiResponse.status != "success")
            {
                string errorMag = apiResponse.message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }
    }
}
