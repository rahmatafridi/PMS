using System.Collections.Generic;

namespace ds.pms.dal.Interface
{
    public interface IGenericRepository<TEntity>
    {
        /// <summary>
        /// IQueryable interface to make queries.
        /// </summary>   
        System.Linq.IQueryable<TEntity> AsQueryable();

        /// <summary>
        /// Get one entity by id.
        /// </summary>   
        TEntity GetById(object id);

        /// <summary>
        /// Insert new entity and get it back.
        /// </summary>   
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Insert new entity and get generated identifier back.
        /// </summary>   
        TId Insert<TId>(TEntity entity) where TId : struct;

        /// <summary>
        /// Insert list of entities using BulkCopy.
        /// </summary>   
        bool Insert(List<TEntity> listOfEntities);

        /// <summary>
        /// Update entity.
        /// </summary>   
        TEntity Update(TEntity entity);

        /// <summary>
        /// Insert or update entity and get it back.
        /// </summary>   
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// Delete entity.
        /// </summary>   
        bool Delete(TEntity entity);

        /// <summary>
        /// Delete entity by id.
        /// </summary>
        bool DeleteById(object id);
    }
}
