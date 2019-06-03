using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Js.webCrawler
{
    class Program
    {

        
        static void Main(string[] args)
        {
         //   Task.Run(new Action(Tiendas.Abcdin.StartProcess));
            Task.Run(new Action(Tiendas.Falabella.StartProcess));


            Console.ReadKey();
        }


    }
}
