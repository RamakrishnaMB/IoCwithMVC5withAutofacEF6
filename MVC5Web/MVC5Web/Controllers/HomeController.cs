using BuisnessLayer.Interface;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashBoardFeeServices _IDashBoardFeeServices;
        public HomeController(IDashBoardFeeServices IDashBoardFeeServices)
        {
            _IDashBoardFeeServices = IDashBoardFeeServices;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetFeeDetails()
        {
            List<FeeDetails> feeDetails = new List<FeeDetails>();
            feeDetails = _IDashBoardFeeServices.GetfeeDetails();
            return View(feeDetails);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}