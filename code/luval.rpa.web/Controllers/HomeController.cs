using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using luval.rpa.web.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using luval.rpa.common.rules.configuration;
using luval.rpa.common.extractors.bp;
using luval.rpa.rules.BP;
using luval.rpa.rules.bp;
using luval.rpa.common.rules;

namespace luval.rpa.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost()]
        public FileResult Upload(IFormFile fileUpload, object termsCheck)
        {
            if (fileUpload == null || fileUpload.Length <= 0) throw new ArgumentException("File not present");
            var bytes = new List<byte>();
            
            using (var inputSteam = fileUpload.OpenReadStream())
            {
                using (var outputStream = new MemoryStream())
                {
                    GetReportStream(inputSteam, outputStream);
                    bytes.AddRange(outputStream.ToArray());
                    return File(bytes.ToArray(), @"application/octet-stream", "result.xlsx");
                }
            }
        }

        private void GetReportStream(Stream inputFile, MemoryStream outputSream)
        {
            var xml = default(string);
            using (var streamReader = new StreamReader(inputFile))
            {
                xml = streamReader.ReadToEnd();
            }
            var extractor = new ReleaseExtractor(xml);
            extractor.Load();
            var bpRunner = new BPRunner();
            var profile = RuleProfile.LoadFromFile();
            var rules = new RuleExtractor().GetRulesFromAssembly(typeof(StageHelper).Assembly);
            var results = bpRunner.RunProfile(profile, extractor.Release);
            var report = new ExcelOutputGenerator();
            report.CreateReport(outputSream, profile, rules, results, extractor.Release);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TermsOfService()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
