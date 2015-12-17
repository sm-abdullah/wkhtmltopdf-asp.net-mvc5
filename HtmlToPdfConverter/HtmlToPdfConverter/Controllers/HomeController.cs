using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HtmlToPdfConverter.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetPDFByURL(string URL) 
        {
            //Get wkhtmltopdf.exe absolute path as i have placed wkhtmltopdf binaries in APP_Data Folder  
            string path = Path.Combine(Server.MapPath("~/App_Data/"), @"wkhtmltopdf\bin\wkhtmltopdf.exe");
            //validate the URL it should be complete URL Like http://www.codingsack.com
            var validURL = new Uri(URL);
            //PDF Bytes from PDF Converter
            byte[] pdfFileBytes = new PDFConverter().Convert(validURL, path);
            //Return Pdf To client
            return File(pdfFileBytes, "application/pdf");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetPDFByHTML(string htmlCode)
        {

            //Get wkhtmltopdf.exe absolute path as i have placed wkhtmltopdf binaries in APP_Data Folder  
            string path = Path.Combine(Server.MapPath("~/App_Data/"), @"wkhtmltopdf\bin\wkhtmltopdf.exe");
           
            byte[] pdfFileBytes = new PDFConverter().Convert(htmlCode, path);
            return File(pdfFileBytes, "application/pdf");

        }
	}
}