using System;
using System.Text;

namespace PrintData
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                new PrintDataDemo().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("_________________________________________");
                while (ex != null)
                {
                    Console.WriteLine(string.Format("{0}\n{1}\n_________________________________________", ex.GetType().ToString(), ex.Message));
                    ex = ex.InnerException;
                }
            }
            finally
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    Console.WriteLine("Press any key to continue . . .");
                    Console.ReadKey(true);
                }
            }
        }
    }
}
