using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ViewModel;

namespace WebAPI.Extensions
{
    public static class ErrorResponseExtension
    {
        /// <summary>
        /// Trata e retorna em um ErrorResponseViewModel um Exception gerado em uma requisição
        /// </summary>
        public static ErrorResponseViewModel ErrorResponse(this Exception exc)
        {
            // Caso não existe exception, retorna null
            if (exc == null)            
                return null;
            
            // Caso existir exception, realiza a tradução recursiva populando o InnerError do ErrorResponse com ErrorReponses de acordo com as InnerExceptions
            return new ErrorResponseViewModel
            {
                Codigo = exc.HResult,
                Mensagem = exc.Message,
                InnerError = ErrorResponse(exc.InnerException)
            };
        }

        /// <summary>
        /// Trata e retorna em um ErrorResponseViewModel um erro de ModelState recebido não consistente em uma requisição
        /// </summary>
        public static ErrorResponseViewModel ErrorResponse(this ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(m => m.Errors);

            return new ErrorResponseViewModel
            {
                Codigo = 100,
                Mensagem = "Não foi possível consistir a mensagem recebida com o modelo esperado.",
                Detalhes = erros.Select(e => e.ErrorMessage).ToArray()
            };
        }
    }
}
