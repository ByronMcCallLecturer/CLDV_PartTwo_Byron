using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SemesterTwo.Models;
using SemesterTwo.Services;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SemesterTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableService _tableService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;

        public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
        {
            _blobService = blobService;
            _tableService = tableService;
            _queueService = queueService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View(new CustomerProfile());
        }

        // Adds Customer Profile to Azure Table
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                await _tableService.AddEntityAsync(profile);
                ViewBag.Message = "Customer profile added successfully!";
            }
            return View("Index", profile);
        }

        // Uploads image to Azure Blob Storage
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                await _blobService.UploadBlobAsync("product-images", file.FileName, stream);
                ViewBag.Message = "Image uploaded successfully!";
            }
            return View("Index");
        }

        // Sends a message to the Azure Queue
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                await _queueService.SendMessageAsync("order-processing", $"Processing order {orderId}");
                ViewBag.Message = "Order processed successfully!";
            }
            return View("Index");
        }

        // Uploads file to Azure File Share
        [HttpPost]
        public async Task<IActionResult> UploadFileToAzure(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                await _fileService.UploadFileAsync("fileshare", file.FileName, stream);
                ViewBag.Message = "File uploaded successfully!";
            }
            return View("Index");
        }
    }
}
