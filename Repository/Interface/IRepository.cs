using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Inclusão da entidade na base de dados
        /// </summary>
        TEntity Add(TEntity entity);

        //void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Carrega a entidade da base de dados a partir de um identificador
        /// </summary>
        TEntity ById(long id);

        /// <summary>
        /// Carrega todos os itens da entidade
        /// </summary>
        IEnumerable<TEntity> All();

        /// <summary>
        /// Atualiza a entidade na base de dados
        /// </summary>
        bool Update(TEntity entity);

        /// <summary>
        /// Atualiza entidades em lote na base de dados
        /// </summary>
        //void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Remove a entidade da base de dados
        /// </summary>
        bool Delete(long id);

        /// <summary>
        /// Remove entidades em lote da base de dados
        /// </summary>
        //void RemoveRange(IEnumerable<TEntity> entities);
    }
}
