﻿using EShopAdminApplication.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace EShopAdminApplication.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ImportUsers(IFormFile file)
        {
            //make a copy 
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            //read data from copy file
            List<User> users = GetAllUsersFromFile(file.FileName);

            HttpClient client = new HttpClient();
            string url = "https://localhost:44329/api/Admin/ImportAllUsers";
            
            HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            var data = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index","Order");
        }

        private List<User> GetAllUsersFromFile(string fileName)
        {
            List<User> users = new List<User>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using ( var stream= System.IO.File.Open(filePath, FileMode.Open,FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        users.Add(new Models.User
                        {
                            Email=reader.GetValue(0).ToString(),
                            Name=reader.GetValue(1).ToString(),
                            LastName = reader.GetValue(2).ToString(),
                            PhoneNumber = reader.GetValue(3).ToString(),
                            Password = reader.GetValue(4).ToString(),
                            ConfirmPassword = reader.GetValue(5).ToString(),
                        });
                    }
                }
            }
            return users;    
        }
    }
}
