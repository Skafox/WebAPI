using Domain.Enum;
using Domain.Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class SystemPermissionService : IPermissionService
    {


        public Dictionary<string, SystemPermissionEnum> GetPermissionDictionary()
        {
            Dictionary<string, SystemPermissionEnum> dicionarioRetorno = new Dictionary<string, SystemPermissionEnum>();
            Type permissionType = typeof(SystemPermissionEnum);

            foreach (SystemPermissionEnum permission in Enum.GetValues(permissionType))
            {
                dicionarioRetorno.Add(Enum.GetName(permissionType, permission), permission);
            }

            return dicionarioRetorno;
        }
    }
}
