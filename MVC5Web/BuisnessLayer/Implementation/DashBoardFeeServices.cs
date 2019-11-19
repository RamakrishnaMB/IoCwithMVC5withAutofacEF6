using BuisnessLayer.Interface;
using DataLayer.Interface;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BuisnessLayer.Implementation
{
    public class DashBoardFeeServices : IDashBoardFeeServices
    {
        public IRepository<FeeDetails> _repository;

        public DashBoardFeeServices(IRepository<FeeDetails> repository)
        {
            _repository = repository;
        }
        public List<FeeDetails> GetfeeDetails()
        {
            List<FeeDetails> lstFeeDetails = new List<FeeDetails>();
            lstFeeDetails = _repository.ExecWithStoreProcedure<FeeDetails>("exec uspDashboard").ToList();
            return lstFeeDetails;
        }
    }
}
