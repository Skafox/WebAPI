using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Extensions;

namespace WebAPI.Filtros
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Recupera via Extension a partir de uma exception o ErroResponse de maneira recursiva
            var errorResponse = context.Exception.ErrorResponse();

            // Devolve como IActionResult um erro 500 com o ErrorResponse configurado da API
            context.Result = new ObjectResult(errorResponse) { StatusCode = 500 };
        }
    }
}
