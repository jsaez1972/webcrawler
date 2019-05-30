using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using HtmlAgilityPack;

namespace Js.webCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            startProcess();
            Console.WriteLine("Fi proceso sinc");
            Console.ReadLine();
        }


        private static async Task startProcess()
        {
            var url = "https://www.abcdin.cl/tienda/es/abcdin";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);


            var links = htmlDoc.DocumentNode.Descendants("a")
                .Where(node => node.GetAttributeValue("class", "").Equals("menuLink")).ToList();

            int cuenta = 0;

            int indexInicio = 0;
            foreach (var x in links)
            {
                Console.WriteLine(x.InnerHtml);

                if (x.Id.StartsWith("subcategoryLink_") && x.GetAttributeValue("href", "").Length > 0)
                {
                    Console.WriteLine(x.GetAttributeValue("href", ""));
                    try
                    {
                        cuenta++;

                        int total = await startProductos(x.GetAttributeValue("href", ""), 0);

                        if (total > 24)
                        {
                            for (int i = 24; i <= 24 * (int)Math.Floor(total / 24.0); i = i + 24)
                            {
                                await startProductos(x.GetAttributeValue("href", ""), i);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            Console.WriteLine("total links " +  cuenta);

            Console.ReadLine();
        }


        private static async Task<int> startProductos(string url, int indexInicio)
        {
            url = url + "#facet:&productBeginIndex:" + indexInicio;

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            var spanNum_products = htmlDoc.DocumentNode.Descendants("span")
                .Where(node => node.GetAttributeValue("class", "").Equals("num_products")).FirstOrDefault();


            int posInicial = spanNum_products.InnerText.IndexOf("de ");

            int posFinal = spanNum_products.InnerText.IndexOf("&nbsp;&#41;");

            
            var divs = htmlDoc.DocumentNode.Descendants("div")
                 .Where(node => node.GetAttributeValue("class", "").Equals("product_info")).ToList();


            foreach (var x in divs)
            {
                var name = x.Descendants("a").FirstOrDefault().InnerText;

                var preciointer = x.Descendants("span")
                    .Where(node => node.GetAttributeValue("class", "").Equals("internetPrice"))
                    .FirstOrDefault().InnerText.Replace("&nbsp;$", "").Replace(".", "").Replace("Internet", "");

                var precionorm = x.Descendants("span")
                     .Where(node => node.GetAttributeValue("class", "").Equals("normalPrice"))
                     .FirstOrDefault().InnerText.Replace("&nbsp;$", "").Replace(".", "").Replace("Normal", "");

                new Business.ServicioProducto().Ingresa(name,
                    Convert.ToDecimal(preciointer),
                     Convert.ToDecimal(precionorm)
                    );
            }


            var total = spanNum_products.InnerText.Substring(posInicial + 3, posFinal - posInicial - 4);
            Console.WriteLine("max: " + total);

            return Convert.ToInt32(total);

        }
    }
}
