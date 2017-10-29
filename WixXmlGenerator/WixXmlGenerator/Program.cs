using System;
using System.Collections.Generic;
using System.Linq;
using WixXmlGenerator.Commands;

namespace WixXmlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Any())
                {
                    var clearArgs = new List<string>();

                    foreach (var arg in args)
                    {
                        clearArgs.Add(arg.Replace("\"", "").Trim());
                    }
                    
                    var result = CommandParser.Parse(clearArgs);
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("To learn how to use Wix XML Generator. Please use 'WixXmlGenerator -help' command.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured. " + e.Message);
            }
        }
    }
}
