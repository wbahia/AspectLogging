using PostSharp.Aspects;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace AspectLogging
{
    [Serializable]
    public class TraceWrapper : OnMethodBoundaryAspect
    {
        #region Propriedades
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Metodos Sobrescritos
        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = Stopwatch.StartNew();

            var argValues = new StringBuilder();
            var count = 1;

            foreach (var argument in args.Arguments)
            {
                if (count > 1)
                    argValues.Append(", \n");
                argValues.Append(argument);
                count++;
            }

            Log.InfoFormat("Início do método: {0}. Parâmetros: {1}.", args.Method.Name, argValues);
        }

        public override void OnExit(MethodExecutionArgs args)
        {

            var sw = (Stopwatch)args.MethodExecutionTag;
            sw.Stop();

            Log.InfoFormat("Fim do método: {0}. Tempo de execução: {1} milisegundos.", args.Method.Name, sw.ElapsedMilliseconds);
        }
        #endregion
        
    }
}
