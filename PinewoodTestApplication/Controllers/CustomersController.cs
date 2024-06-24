using PinewoodTestApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PinewoodTestApplication.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            IEnumerable<Customer> customers = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44368/api/"); // Update with your Web API base URL
                var responseTask = client.GetAsync("customers");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Customer>>();
                    readTask.Wait();

                    customers = readTask.Result;
                }
                else
                {
                    customers = Enumerable.Empty<Customer>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(customers);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44368/api/customers"); // Update with your Web API endpoint
                var postTask = client.PostAsJsonAsync<Customer>("customers", customer);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(customer);
                }
            }
        }

        // GET: Customers/Edit/{id}
        public ActionResult Edit(int id)
        {
            Customer customer = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44368/api/");
                var responseTask = client.GetAsync($"customers/{id}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Customer>();
                    readTask.Wait();

                    customer = readTask.Result;
                }
            }

            return View(customer);
        }

        // POST: Customers/Edit/{id}
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44368/api/customers");
                var putTask = client.PutAsJsonAsync<Customer>($"customers/{customer.Id}", customer);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(customer);
                }
            }
        }

        // GET: Customers/Delete/{id}
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44368/api/");
                var deleteTask = client.DeleteAsync($"customers/{id}");
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return RedirectToAction("Index");
                }
            }
        }
    }

}