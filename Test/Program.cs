using FiltersMDXCubeNortwind;
using System;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] clients = new string[] { "Alfreds Futterkiste", "Ana Trujillo Emparedados y helados", "Antonio Moreno Taquería" };
            string[] months = new string[] { "Abril", "Agosto", "Diciembre" };
            string[] years = new string[] { "1996", "1997", "1998" };
            ConsultaMex queryMdx = new ConsultaMex();
            queryMdx.GetChartLabelsDataBarQueryMdx(clients, months, years);

            //string tmpNombre = "[Dim Tiempo].[Dim Tiempo Año].&[1997].[Dim Tiempo].[Dim Tiempo Mes].&[Abril]";
            //tmpNombre = tmpNombre.Replace("[Dim Tiempo].[Dim Tiempo Año].&[", " ");
            //tmpNombre = tmpNombre.Replace("].[Dim Tiempo].[Dim Tiempo Mes].&[", " ");
            //tmpNombre = tmpNombre.Replace("[", " ");
            //tmpNombre = tmpNombre.Replace("]", " ");

            //ChartDataPie chartDataPie = new ChartDataPie();
            //chartDataPie.MesAnio = "Enero 2020";
            //chartDataPie.Quatity = 0;

            //ChartDataPie chartDataPie2 = new ChartDataPie();
            //chartDataPie.MesAnio = "Enero 2020";
            //chartDataPie.Quatity = 0;

            //ChartDataPie chartDataPie3 = new ChartDataPie();
            //chartDataPie.MesAnio = "Enero 2020";
            //chartDataPie.Quatity = 0;

            //Client client = new Client();
            //client.Name = "Juanito";
            //client.LstData = new List<ChartDataPie>();
            //client.LstData.Add(chartDataPie);
            //client.LstData.Add(chartDataPie);
            //client.LstData.Add(chartDataPie);

            //var sss = client;
            Console.WriteLine("");
            Console.ReadKey();

        }
    }
}
