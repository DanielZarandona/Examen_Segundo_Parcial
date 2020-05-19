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
    //Todos los otigenes, todas las cabeceras y todos los verbos de metodos(get, put, post)
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("daozara/nortwind")]//no seas cabron con tu zaranchanclas
    public class NortwindDBController : ApiController
    {


        [HttpGet]
        [Route("GetClients")]
        public HttpResponseMessage GetClients()
        {
            ConsultaMex queryMdx = new ConsultaMex();            
            return Request.CreateResponse(HttpStatusCode.OK, queryMdx.GetClientsQueryMdx());
        }

        [HttpPost]
        [Route("GetDataPie")]
        public HttpResponseMessage GetDataPie([FromBody] DatosAEnviar filterModel)
        {
            ConsultaMex queryMdx = new ConsultaMex();
            return Request.CreateResponse(HttpStatusCode.OK, queryMdx.GetChartDataPieQueryMdx(filterModel.Clients, filterModel.Months, filterModel.Years));
        }



        [HttpPost]
        [Route("GetDataBar")]
        public HttpResponseMessage GetDataBar([FromBody] DatosAEnviar filterModel)
        {

            ConsultaMex queryMdx = new ConsultaMex();
            return Request.CreateResponse(HttpStatusCode.OK, queryMdx.GetChartDataBarQueryMdx(filterModel.Clients,filterModel.Months,filterModel.Years));
        }

        [HttpPost]
        [Route("GetLabelsBar")]
        public HttpResponseMessage GetLabelsBar([FromBody] DatosAEnviar filterModel)
        {
            ConsultaMex queryMdx = new ConsultaMex();
            return Request.CreateResponse(HttpStatusCode.OK, queryMdx.GetChartLabelsDataBarQueryMdx(filterModel.Clients, filterModel.Months, filterModel.Years));
        }
    }
}
