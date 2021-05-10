using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IPermissionService
    {
        Dictionary<string, SystemPermissionEnum> GetPermissionDictionary();
    }
}
