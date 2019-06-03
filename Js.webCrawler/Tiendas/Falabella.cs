using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace Js.webCrawler.Tiendas
{
    public class Falabella
    {

        public static async void StartProcess()
        {
            var url = "http://www.falabella.com/falabella-cl";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");


            var html = await httpClient.GetStringAsync(url);



            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);


            var links = htmlDoc.DocumentNode.Descendants("a")
                .Where(node => node.GetAttributeValue("href", "").Contains("cat")
               && node.GetAttributeValue("class", "").Equals(""))
                .ToList();



            int cuenta = 0;


            foreach (var x in links)
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
            Console.WriteLine("total links " + cuenta);

        }


        private static async Task startProductos(string url, string categoria)
        {
            url = "https://www.falabella.com" + url;
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");


            var html = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);

            //var spanNum_products = htmlDoc.DocumentNode.Descendants("span")
            //    .Where(node => node.GetAttributeValue("class", "").Equals("num_products")).FirstOrDefault();


            //int posInicial = spanNum_products.InnerText.IndexOf("de ");

            //int posFinal = spanNum_products.InnerText.IndexOf("&nbsp;&#41;");


            var scripts = htmlDoc.DocumentNode.Descendants("script")
                
                .ToList ();
            //     .Where(node => node.GetAttributeValue("src", "").Equals("/static/assets/132/scripts/react/productListApp.js?vid=132")).ToList();


            foreach (var x in scripts)
            {
                if (x.InnerHtml.Contains("fbra_browseProductListConfig"))
                {
                    var jListado = x.InnerHtml;

                    jListado = jListado.Replace("var fbra_browseProductListConfig =", "");
                    jListado = jListado.Replace("var fbra_browseProductList = FalabellaReactApplication.createComponent(\"ProductListApp\", fbra_browseProductListConfig);", "");
                    jListado = jListado.TrimStart().TrimEnd();
                    jListado = jListado.Remove(jListado.Length - 1, 1);


                    JObject rss = JObject.Parse(jListado);

                    var resultList = rss["state"]
          ["searchItemList"]["resultList"];


                    foreach (var producto in resultList)
                    {
                        try
                            { 

                        new Business.ServicioProducto().Ingresa(producto["title"].ToString(),
                    Convert.ToDecimal(producto.SelectToken("$.prices[?(@.type == 3)]")["originalPrice"].ToString().Replace(".", "")),
                     Convert.ToDecimal(producto.SelectToken("$.prices[?(@.type == 1)]")["originalPrice"].ToString().Replace(".", "")),
                    categoria,
                    "Falabella",
                  Convert.ToInt32(producto["skuId"].ToString())
                   );

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("err"+ producto["title"].ToString() + producto.ToString () + "=" + ex.ToString());
                        }

                    }



                    break;
                }

            }

        }

    }
}
