using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using HtmlAgilityPack;

namespace Js.webCrawler.Tiendas
{
    public class Abcdin
    {

        public  static async void StartProcess()
        {
            var url = "https://www.abcdin.cl/tienda/es/abcdin";


            var html = await new HttpClient().GetStringAsync(url);
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);


            var links = htmlDoc.DocumentNode.Descendants("a")
                .Where(node => node.GetAttributeValue("class", "").Equals("menuLink")).ToList();

            int cuenta = 0;


            foreach (var x in links)
            {
                Console.WriteLine(x.InnerHtml);

                if (x.Id.StartsWith("subcategoryLink_") && x.GetAttributeValue("href", "").Length > 0)
                {
                    try
                    {
                        cuenta++;

                        await startProductos(x.GetAttributeValue("href", ""), x.InnerText);



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            Console.WriteLine("total links " + cuenta);


        }


        private static async Task startProductos(string url, string categoria)
        {
            url = url + "#facet:&productBeginIndex:0&orderBy:&pageView:grid&minPrice:&maxPrice:&pageSize:100";

            var html = await new HttpClient().GetStringAsync(url);
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
                     Convert.ToDecimal(precionorm),
                      Convert.ToDecimal(preciointer),
                     categoria,
                     "ABCDIN",
                     0
                    );
            }


        }

    }
}
