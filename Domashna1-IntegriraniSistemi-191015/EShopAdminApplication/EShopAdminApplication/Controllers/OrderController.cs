using ClosedXML.Excel;
using EShopAdminApplication.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace EShopAdminApplication.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        } 
        public IActionResult Index()
        {
            HttpClient client =new HttpClient();
            string url = "https://localhost:44329/api/Admin/GetAllActiveOrders";
            HttpResponseMessage response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;

            return View(data);
        }
        public IActionResult Details(Guid orderId)
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44329/api/Admin/GetDetailsForOrder";
            var model = new
            {
                Id = orderId
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model),Encoding.UTF8,"application/json");
            HttpResponseMessage response = client.PostAsync(url,content ).Result;
            var data = response.Content.ReadAsAsync<Order>().Result;

            return View(data);
        }
        public FileContentResult CreateInvoice(Guid id)
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44329/api/Admin/GetDetailsForOrder";
            var model = new
            {
                Id = id
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            var data = response.Content.ReadAsAsync<Order>().Result;
            var templetePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templetePath);
            document.Content.Replace("{{OrderNumber}}", data.Id.ToString());
            document.Content.Replace("{{UserName}}", data.User.UserName);
            StringBuilder sb=new StringBuilder();
            var totalPrice = 0.0;
            foreach(var item in data.Bilets)
            {
                totalPrice += item.Quantity * item.SelectedBilet.BilletPrice;
                sb.AppendLine("Movie name: "+ item.SelectedBilet.MovieName+ " Quantity: "+item.Quantity+ " Rating: "+ item.SelectedBilet.MovieRating+" Price: "+ item.SelectedBilet.BilletPrice+" den. " ); 
            }
            document.Content.Replace("{{ProductList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString()+ "den. ");

            var stream= new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(),new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            using(var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");
                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Customer Email";
                HttpClient client = new HttpClient();
                string url = "https://localhost:44329/api/Admin/GetAllActiveOrders";
                HttpResponseMessage response = client.GetAsync(url).Result;
                var data = response.Content.ReadAsAsync<List<Order>>().Result;
                for(int i = 1; i <= data.Count; i++)
                {
                    var item = data[i-1];
                    worksheet.Cell(i+1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;
                    for(int p = 0; p < item.Bilets.Count(); p++)
                    {
                        worksheet.Cell(1, p+3).Value =("Product-"+(p+1) );
                        worksheet.Cell(i + 1, p + 3).Value = item.Bilets.ElementAt(p).SelectedBilet.MovieName;

                    }
                }
                using(var stream = new MemoryStream()) {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
                
            } 
           
        }
    }
}
