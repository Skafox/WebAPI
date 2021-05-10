using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    /// <summary>
    /// Enum para definição das claims customizadas do sistema
    /// </summary>
    public enum CustomClaimTypeEnum
    {        
        Sid,
        NameIdentifier,
        Name,
        Email,
        Role
    }
}
