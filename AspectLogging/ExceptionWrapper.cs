using log4net;
using PostSharp.Aspects;
using System;
using System.Reflection;
using System.Text;

namespace AspectLogging
{
    [Serializable]
    public class ExceptionWrapper : OnExceptionAspect
    {
        #region Propriedades

        private static readonly ILog Log =LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Type _bypassedException;

        #endregion
        
        #region Construtor
        /// <summary>
        /// Construtor da classe ExceptionWrapper
        /// </summary>
        /// <param name="bypassExceptionType">Tipo de exceção que deve ser ignorado. Nulo será ignorado também.</param>
        public ExceptionWrapper(Type bypassExceptionType)
        {
            _bypassedException = bypassExceptionType;
        }

        public ExceptionWrapper()
        {
            _bypassedException = null;
        }
        #endregion

        #region Metodos Sobrescritos
        /// <summary>
        /// Metodo sobrescrito que captura as excecoes
        /// </summary>
        /// <param name="args">Argumentos</param>
        public override void OnException(MethodExecutionArgs args)
        {

            if (_bypassedException != null && args.Exception.GetType() == _bypassedException)
            {
                args.FlowBehavior = FlowBehavior.Return;
                return;
            }

            args.FlowBehavior = FlowBehavior.Continue;
            var errorId = Guid.NewGuid();
            var argValues = new StringBuilder();
            var logMessage = new StringBuilder();
            var count = 1;


            foreach (var argument in args.Arguments)
            {
                if (count > 1)
                    argValues.Append(", \n");
                argValues.Append(argument);
                count++;
            }

            // mensagem para o arquivo de log/email/bd
            logMessage.Append(String.Format("Ocorreu um erro durante a execução do programa. O código de erro gerado é: {0}", errorId.ToString().ToUpper()));
            logMessage.Append("\t\n" + String.Format("Método: [{0}]", args.Method.Name));
            logMessage.Append("\n" + String.Format("Parâmetros: {0}", argValues));
            logMessage.Append("\n" + String.Format("Exceção: {0}", args.Exception));

            Log.Error(logMessage);


            // mensagem lancada para a aplicacao
            var clientMessage = String.Format("Por favor entre em contato com o suporte informando o seguinte código de erro: {0}", errorId.ToString().ToUpper());
            throw new Exception(clientMessage);

        }
        #endregion
        
    }
}
