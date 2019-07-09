using System;
using AspectLogging;


namespace AspectLoggingConsoleTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try
            {
                CreateError("PARAMETRO_CHAMADA_METODO");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            Console.ReadKey();

        }


        [ExceptionWrapper(typeof(ApplicationException))]
        public static void CreateError(string s)
        {
            try
            {
                throw new ApplicationException("EXCEPTION");
            }
            catch (Exception ex)
            {
                    
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }
    }
}
