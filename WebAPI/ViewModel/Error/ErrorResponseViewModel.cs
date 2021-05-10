using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.ViewModel
{
    public class ErrorResponseViewModel
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public ErrorResponseViewModel InnerError { get; set; }
        public string[] Detalhes { get; set; }
    }
}
