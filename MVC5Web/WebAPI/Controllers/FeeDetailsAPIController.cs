using BuisnessLayer.Interface;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Utilites;

namespace WebAPI.Controllers
{
    public class FeeDetailsAPIController : ApiController
    {
        private readonly IDashBoardFeeServices _IDashBoardFeeServices;

        public FeeDetailsAPIController(IDashBoardFeeServices IDashBoardFeeServices)
        {
            _IDashBoardFeeServices = IDashBoardFeeServices;
        }


        [HttpGet]
        [Route("api/Fee/AllDetails")]
        public HttpResponseMessage GetFeeDetails()
        {
            //http://localhost:59881/api/FeeDetailsAPI/GetFeeDetails
            //var DeviceId = ((JObject)jObject).GetValue("deviceId", StringComparison.OrdinalIgnoreCase).Value<string>();
            //var GradeId = ((JObject)jObject).GetValue("gradeId", StringComparison.OrdinalIgnoreCase).Value<int>();
            List<FeeDetails> feeDetails = new List<FeeDetails>();
            feeDetails = _IDashBoardFeeServices.GetfeeDetails();


            //method: post, url : http://localhost:30888/api/Subject/getSubjectsForExamPaper , Request body: { userId:10,deviceId:11,gradeId:22} , Content-Type: application/json            

            return Request.CreateResponse(HttpStatusCode.OK, new APIMessage { StatusCode = (int)HttpStatusCode.OK, StatusMessage = "Sucess great !!", Data = feeDetails });

        }
    }
}
