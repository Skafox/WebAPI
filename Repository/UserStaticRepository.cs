using Domain.Enum;
using Domain.Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class UserStaticRepository : IRepository<User>
    {
        public List<User> lstUsers = new List<User>();

        public UserStaticRepository()
        {
            // Cria usuários comuns
            for (int i = 0; i < 2; i++)
            {
                lstUsers.Add(new User { 
                    ID=i, 
                    Username="User" + i.ToString(), 
                    Name="Usuário " + i.ToString(),
                    Password="123", 
                    Permission=(int)SystemPermissionEnum.Common 
                });
            }

            // Cria usuário administrador
            lstUsers.Add(new User
            {
                ID = -1,
                Username = "User Admin",
                Name = "Usuario Administrador",
                Password = "adm",
                Permission = (int)SystemPermissionEnum.Admin
            });
        }

        /// <summary>
        /// Inclusão da entidade na base de dados
        /// </summary>
        public User Add(User user)
        {
            user.ID = lstUsers.Max(u => u.ID) + 1;

            if (lstUsers.All(u => u.Username != user.Username))
            {
                lstUsers.Add(user);
                return user;
            }

            return user;
        }

        /// <summary>
        /// Carrega a entidade da base de dados a partir de um identificador
        /// </summary>
        public User ById(long id)
        {
            return this.lstUsers.FirstOrDefault(u => u.ID == id);
        }

        /// <summary>
        /// Carrega todos os itens da entidade
        /// </summary>
        public IEnumerable<User> All()
        {
            return this.lstUsers;
        }

        /// <summary>
        /// Atualiza a entidade na base de dados
        /// </summary>
        public bool Update(User newUser)
        {
            if (newUser.ID > 0)
            {
                var oldUser = lstUsers.FirstOrDefault(u => u.ID == newUser.ID);
                oldUser.Permission = newUser.Permission;
                oldUser.Password = newUser.Password;
                oldUser.Name = newUser.Name;

                return true;
            }                

            return false;
        }

        /// <summary>
        /// Remove a entidade da base de dados
        /// </summary>
        public bool Delete(long id)
        {
            User usuarioRemover = lstUsers.FirstOrDefault(u => u.ID == id);
            this.lstUsers.Remove(usuarioRemover);

            return true;
        }

    }
}
