using Microsoft.AnalysisServices.AdomdClient;
using System.Collections.Generic;
using System.Data;

namespace FiltersMDXCubeNortwind
{
    //C:\Program Files\Microsoft.NET\ADOMD.NET\150\Microsoft.AnalysisServices.AdomdClient.dll
    public class ConsultaMex
    {
        //Obtener Nombre de Clientes
        public DataTable GetClientsQueryMdx()
        {
            string dimension = "";
            DataTable dataTable = new DataTable();
            dimension = "[Dim Cliente].[Dim Cliente Nombre]";
            List<ChartDataPie> LstChartDataPie = new List<ChartDataPie>();
            var mdxQuery = $@"
SELECT NON EMPTY {{ [Measures].[Fact Ventas Netas] }}
 ON COLUMNS, NON EMPTY {{ ( {dimension}.Children) }} 
ON ROWS FROM [DWH Northwind]";
            using (AdomdConnection cnn = new AdomdConnection($@"Provider=MSOLAP; Data Source=localhost;Catalog=Cubo811; User ID=sa; Password = daniel; Persist Security Info = True; Impersonation Level = Impersonate"))
            {
                AdomdDataAdapter adomdDataAdapter;
                cnn.Open();
                using (AdomdCommand cmd = new AdomdCommand(mdxQuery, cnn))
                {
                    adomdDataAdapter = new AdomdDataAdapter(cmd);
                    adomdDataAdapter.Fill(dataTable);
                    dataTable.Columns[0].ColumnName = "ClientesNombre";
                    dataTable.Columns[1].ColumnName = "FactVentas";
                }
            }
            return dataTable;
        }

        public DataTable GetChartDataPieQueryMdx(string[] clients, string[] months, string[] years)
        {
            
            string SelectedClients = "", SelectedMonths = "", SelectedYears = "";
            if (clients.Length >= 1)
            {
                foreach (var item in clients)
                {
                    SelectedClients += $@"[Dim Cliente].[Dim Cliente Nombre].&[{item}],";
                }
                SelectedClients = SelectedClients.Remove(SelectedClients.Length - 1);
            }
            if (months.Length >= 1)
            {
                foreach (var item in months)
                {
                    SelectedMonths += $@"[Dim Tiempo].[Dim Tiempo Mes].&[{item}],";
                }
                SelectedMonths = SelectedMonths.Remove(SelectedMonths.Length - 1);
            }
            if (years.Length >= 1)
            {
                foreach (var item in years)
                {
                    SelectedYears += $@"[Dim Tiempo].[Dim Tiempo Año].&[{item}],";
                }
                SelectedYears = SelectedYears.Remove(SelectedYears.Length - 1);
            }
            DataTable dataTable = new DataTable();
            List<ChartDataPie> LstChartDataPie = new List<ChartDataPie>();
            var mdxQuery = $@"
 SELECT NON EMPTY {{

 (
  [Dim Tiempo].[Dim Tiempo Año].[Dim Tiempo Año].ALLMEMBERS *
  [Dim Tiempo].[Dim Tiempo Mes].[Dim Tiempo Mes].ALLMEMBERS
 )
 }}
 ON COLUMNS, NON EMPTY {{ (
 (
  [Measures].[Fact Ventas Netas],
 [Dim Cliente].[Dim Cliente Nombre].[Dim Cliente Nombre].ALLMEMBERS
 )
 )
 }}  ON ROWS FROM  ( SELECT ( {{
{SelectedYears}
 }}) ON COLUMNS FROM ( SELECT ( {{
{SelectedMonths}
 }} ) ON COLUMNS FROM ( SELECT ( {{
{SelectedClients} 
 }} ) ON COLUMNS 
 FROM [DWH Northwind]))) 
";
            using (AdomdConnection cnn = new AdomdConnection($@"Provider=MSOLAP; Data Source=localhost;Catalog=Cubo811; User ID=sa; Password = roverto; Persist Security Info = True; Impersonation Level = Impersonate"))
            {
                AdomdDataAdapter adomdDataAdapter;
                cnn.Open();
                using (AdomdCommand cmd = new AdomdCommand(mdxQuery, cnn))
                {
                    adomdDataAdapter = new AdomdDataAdapter(cmd);
                    adomdDataAdapter.Fill(dataTable);
                }
                dataTable.Columns.RemoveAt(0);
                dataTable.Columns[0].ColumnName = "ClientesNombre";
                dataTable.Columns.Add("Total", typeof(double));
                
                
                double r = 0;
                double sum = 0;

                for (int renglones = 0; renglones < dataTable.Rows.Count; renglones++)
                {
                    for (int columnas = 0; columnas < dataTable.Columns.Count; columnas++)
                    {
                        double.TryParse(dataTable.Rows[renglones][columnas].ToString(), out r);
                        sum = sum + r;
                    }
                    dataTable.Rows[renglones][dataTable.Columns.Count - 1] = sum;
                    sum = 0;
                }

                List<int> numeros = new List<int>();
                for (int i = 1; i < dataTable.Columns.Count - 1; i++)
                {
                    numeros.Add(i);
                }

                foreach (var item in numeros)
                {
                    if (dataTable.Columns.Count == 2)
                    {
                        break;
                    }
                    dataTable.Columns.RemoveAt(dataTable.Columns.Count - 2);

                }
            }
            return dataTable;
        }

        public List<dynamic> GetChartDataBarQueryMdx(string[] clients, string[] months, string[] years) //
        {
            List<dynamic> dlist = new List<dynamic>();
            List<string> lstColumNames = new List<string>();

            string SelectedClients = "", SelectedMonths = "", SelectedYears = "";

            if (clients.Length >= 1)
            {
                foreach (var item in clients)
                {
                    SelectedClients += $@"[Dim Cliente].[Dim Cliente Nombre].&[{item}],";
                }
                SelectedClients = SelectedClients.Remove(SelectedClients.Length - 1);
            }
            if (months.Length >= 1)
            {
                foreach (var item in months)
                {
                    SelectedMonths += $@"[Dim Tiempo].[Dim Tiempo Mes].&[{item}],";
                }
                SelectedMonths = SelectedMonths.Remove(SelectedMonths.Length - 1);
            }
            if (years.Length >= 1)
            {
                foreach (var item in years)
                {
                    SelectedYears += $@"[Dim Tiempo].[Dim Tiempo Año].&[{item}],";
                }
                SelectedYears = SelectedYears.Remove(SelectedYears.Length - 1);
            }
            DataTable dataTable = new DataTable();
            List<ChartDataPie> LstChartDataPie = new List<ChartDataPie>();
            var mdxQuery = $@"

 SELECT NON EMPTY {{

 (
  [Dim Tiempo].[Dim Tiempo Año].[Dim Tiempo Año].ALLMEMBERS *
  [Dim Tiempo].[Dim Tiempo Mes].[Dim Tiempo Mes].ALLMEMBERS
 )
 }}
 ON COLUMNS, NON EMPTY {{ (
 (
  [Measures].[Fact Ventas Netas],
 [Dim Cliente].[Dim Cliente Nombre].[Dim Cliente Nombre].ALLMEMBERS
 )
 )
 }}  ON ROWS FROM  ( SELECT ( {{
{SelectedYears}
 }}) ON COLUMNS FROM ( SELECT ( {{
{SelectedMonths}
 }} ) ON COLUMNS FROM ( SELECT ( {{
{SelectedClients} 
 }} ) ON COLUMNS 
 FROM [DWH Northwind]))) 
";
            using (AdomdConnection cnn = new AdomdConnection($@"Provider=MSOLAP; Data Source=localhost;Catalog=Cubo811; User ID=sa; Password = roverto; Persist Security Info = True; Impersonation Level = Impersonate"))
            {
                AdomdDataAdapter adomdDataAdapter;
               
                cnn.Open();
                using (AdomdCommand cmd = new AdomdCommand(mdxQuery, cnn))
                {
                    adomdDataAdapter = new AdomdDataAdapter(cmd);
                    adomdDataAdapter.Fill(dataTable);
                }

                
                string tmpNombre = "Cliente";
                dataTable.Columns.RemoveAt(0);
                dataTable.Columns[0].ColumnName = "Cliente";
                foreach (DataColumn item in dataTable.Columns)
                {
                    tmpNombre = item.ColumnName;
                    tmpNombre = tmpNombre.Replace("[Dim Tiempo].[Dim Tiempo Año].&[", " ");
                    tmpNombre = tmpNombre.Replace("].[Dim Tiempo].[Dim Tiempo Mes].&[", " ");
                    tmpNombre = tmpNombre.Replace("[", " ");
                    tmpNombre = tmpNombre.Replace("]", " ");                    
                    dataTable.Columns[item.Ordinal].ColumnName = tmpNombre;
                    lstColumNames.Add(tmpNombre);
                    tmpNombre = "";
                }


                List<double> lstDouble = null;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    lstDouble = new List<double>();
                    string ssss="";
                    foreach (var item2 in lstColumNames)
                    {

                        
                        double number1 = 0;
                        var dataToConvert = "";
                        
                        if (dataTable.Rows[i][item2].ToString().Equals(""))
                        {
                            dataToConvert = "0";
                        }
                        else
                        {
                            dataToConvert = dataTable.Rows[i][item2].ToString();
                        }
                        bool canConvert = double.TryParse(dataToConvert, out number1);
                        if (canConvert == true)
                        {
                            lstDouble.Add(number1);
                        }

                        else
                        {
                            ssss= dataTable.Rows[i][item2].ToString();
                        }
                            
                    }

                    dynamic objTabla = new
                    {
                        label = ssss,
                        data = lstDouble
                    };
                    dlist.Add(objTabla);
                    //lstDouble.Clear();
                }
                var dhyudsyuyuds = dlist;
                                                   
            }

            dynamic Labels = new
            {
                labelColums = lstColumNames                
            };
            lstColumNames.RemoveAt(0);
            var asdasd = lstColumNames;
            var gagag = dataTable;
            //dlist.Add(Labels);
            return dlist;
        }

        public List<string> GetChartLabelsDataBarQueryMdx(string[] clients, string[] months, string[] years) //
        {
            List<dynamic> dlist = new List<dynamic>();
            List<string> lstColumNames = new List<string>();

            string SelectedClients = "", SelectedMonths = "", SelectedYears = "";

            if (clients.Length >= 1)
            {
                foreach (var item in clients)
                {
                    SelectedClients += $@"[Dim Cliente].[Dim Cliente Nombre].&[{item}],";
                }
                SelectedClients = SelectedClients.Remove(SelectedClients.Length - 1);
            }
            if (months.Length >= 1)
            {
                foreach (var item in months)
                {
                    SelectedMonths += $@"[Dim Tiempo].[Dim Tiempo Mes].&[{item}],";
                }
                SelectedMonths = SelectedMonths.Remove(SelectedMonths.Length - 1);
            }
            if (years.Length >= 1)
            {
                foreach (var item in years)
                {
                    SelectedYears += $@"[Dim Tiempo].[Dim Tiempo Año].&[{item}],";
                }
                SelectedYears = SelectedYears.Remove(SelectedYears.Length - 1);
            }
            DataTable dataTable = new DataTable();
            List<ChartDataPie> LstChartDataPie = new List<ChartDataPie>();
            var mdxQuery = $@"

 SELECT NON EMPTY {{

 (
  [Dim Tiempo].[Dim Tiempo Año].[Dim Tiempo Año].ALLMEMBERS *
  [Dim Tiempo].[Dim Tiempo Mes].[Dim Tiempo Mes].ALLMEMBERS
 )
 }}
 ON COLUMNS, NON EMPTY {{ (
 (
  [Measures].[Fact Ventas Netas],
 [Dim Cliente].[Dim Cliente Nombre].[Dim Cliente Nombre].ALLMEMBERS
 )
 )
 }}  ON ROWS FROM  ( SELECT ( {{
{SelectedYears}
 }}) ON COLUMNS FROM ( SELECT ( {{
{SelectedMonths}
 }} ) ON COLUMNS FROM ( SELECT ( {{
{SelectedClients} 
 }} ) ON COLUMNS 
 FROM [DWH Northwind]))) 
";
            using (AdomdConnection cnn = new AdomdConnection($@"Provider=MSOLAP; Data Source=localhost;Catalog=Cubo811; User ID=sa; Password = roverto; Persist Security Info = True; Impersonation Level = Impersonate"))
            {
                AdomdDataAdapter adomdDataAdapter;

                cnn.Open();
                using (AdomdCommand cmd = new AdomdCommand(mdxQuery, cnn))
                {
                    adomdDataAdapter = new AdomdDataAdapter(cmd);
                    adomdDataAdapter.Fill(dataTable);
                }


                string tmpNombre = "Cliente";
                dataTable.Columns.RemoveAt(0);
                dataTable.Columns[0].ColumnName = "Cliente";
                foreach (DataColumn item in dataTable.Columns)
                {
                    tmpNombre = item.ColumnName;
                    tmpNombre = tmpNombre.Replace("[Dim Tiempo].[Dim Tiempo Año].&[", " ");
                    tmpNombre = tmpNombre.Replace("].[Dim Tiempo].[Dim Tiempo Mes].&[", " ");
                    tmpNombre = tmpNombre.Replace("[", " ");
                    tmpNombre = tmpNombre.Replace("]", " ");
                    dataTable.Columns[item.Ordinal].ColumnName = tmpNombre;
                    lstColumNames.Add(tmpNombre);
                    tmpNombre = "";
                }


                List<double> lstDouble = null;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    lstDouble = new List<double>();
                    string ssss = "";
                    foreach (var item2 in lstColumNames)
                    {


                        double number1 = 0;
                        var dataToConvert = "";

                        if (dataTable.Rows[i][item2].ToString().Equals(""))
                        {
                            dataToConvert = "0";
                        }
                        else
                        {
                            dataToConvert = dataTable.Rows[i][item2].ToString();
                        }
                        bool canConvert = double.TryParse(dataToConvert, out number1);
                        if (canConvert == true)
                        {
                            lstDouble.Add(number1);
                        }

                        else
                        {
                            ssss = dataTable.Rows[i][item2].ToString();
                        }

                    }

                    dynamic objTabla = new
                    {
                        label = ssss,
                        data = lstDouble
                    };
                    dlist.Add(objTabla);
                    //lstDouble.Clear();
                }
                var dhyudsyuyuds = dlist;

            }

            dynamic Labels = new
            {
                labelColums = lstColumNames
            };
            lstColumNames.RemoveAt(0);
            var asdasd = lstColumNames;
            var gagag = dataTable;
            //dlist.Add(Labels);
            return lstColumNames;
        }

    }
}
