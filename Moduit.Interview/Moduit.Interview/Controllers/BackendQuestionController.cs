using Microsoft.AspNetCore.Mvc;
using Moduit.CallApiHelper;
using Moduit.Domain.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Moduit.Interview.Controllers
{
    [Route("backend")]
    [ApiController]
    public class BackendQuestionController : ControllerBase
    {
        private readonly CallApiModuitHelper CallApiModuitHelper;
        public BackendQuestionController(CallApiModuitHelper callApiModuitHelper)
        {
            CallApiModuitHelper = callApiModuitHelper;
        }

        [HttpGet]
        [Route("question/one")]
        public Product GetProduct()
        {
            return this.CallApiModuitHelper.GetProduct();
        }

        [HttpGet]
        [Route("question/two")]
        public IEnumerable<Product> GetProducts()
        {
            return this.CallApiModuitHelper.GetProductsFiltered();
        }

        [HttpGet]
        [Route("question/three")]
        public IEnumerable<Product> GetProductsFlatten()
        {
            return this.CallApiModuitHelper.GetProductsFlatten();
        }
    }
}
