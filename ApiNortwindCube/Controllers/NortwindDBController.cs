using FiltersMDXCubeNortwind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiNortwindCube.Controllers
{
   
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("isscjrmp/nortwind")]
    public class NortwindDBController : ApiController
    {


        [HttpGet]
        [Route("GetClients")]
        public HttpResponseMessage GetClients()
        {
            QueryMdx queryMdx = new QueryMdx();            
            return Request.CreateResponse(HttpStatusCode.OK, queryMdx.GetClientsQueryMdx());
        }

        [HttpPost]
        [Route("GetDataPie")]
        public HttpResponseMessage GetDataPie([FromBody] FilterModel filterModel)
        {
            QueryMdx queryMdx = new QueryMdx();
            return Request.CreateResponse(HttpStatusCode.OK, queryMdx.GetChartDataPieQueryMdx(filterModel.Clients, filterModel.Months, filterModel.Years));
        }



        [HttpPost]
        [Route("GetDataBar")]
        public HttpResponseMessage GetDataBar([FromBody] FilterModel filterModel)
        {

            QueryMdx queryMdx = new QueryMdx();
            return Request.CreateResponse(HttpStatusCode.OK, queryMdx.GetChartDataBarQueryMdx(filterModel.Clients,filterModel.Months,filterModel.Years));
        }

        [HttpPost]
        [Route("GetLabelsBar")]
        public HttpResponseMessage GetLabelsBar([FromBody] FilterModel filterModel)
        {
            QueryMdx queryMdx = new QueryMdx();
            return Request.CreateResponse(HttpStatusCode.OK, queryMdx.GetChartLabelsDataBarQueryMdx(filterModel.Clients, filterModel.Months, filterModel.Years));
        }
    }
}
