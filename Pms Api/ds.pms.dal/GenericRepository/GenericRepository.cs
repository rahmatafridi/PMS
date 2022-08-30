using ds.pms.dal.EntityHelper;
using ds.pms.dal.Interface;
using ds.pms.dal.Models;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ds.pms.dal.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private PmsDB dataConnection;
        //not used private int clientId;
        private string providerName, connectionString;

        public GenericRepository(string sqlConnectionString)
        {
            connectionString = sqlConnectionString;
            //dataConnection = new PmsDB(providerName, connectionString);
        }

        public GenericRepository(string sqlPoviderName, string sqlConnectionString)
        {
            providerName = sqlPoviderName;
            connectionString = sqlConnectionString;
            //dataConnection = new PmsDB(providerName, connectionString);
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return dataConnection.GetTable<TEntity>();
        }

        public TEntity GetById(object id)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                var itemParameter = Expression.Parameter(typeof(TEntity), "item");
                var whereExpression = Expression.Lambda<Func<TEntity, bool>>
                    (
                    Expression.Equal(
                        Expression.Property(
                             itemParameter,
                             typeof(TEntity).GetPrimaryKey().Name
                             ),
                        Expression.Constant(id)
                        ),
                    new[] { itemParameter }
                    );
                return dataConnection.GetTable<TEntity>().Where(whereExpression).FirstOrDefault();
            }
        }
        public TEntity GetByDataId(object id)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                var itemParameter = Expression.Parameter(typeof(TEntity), "item");
                var whereExpression = Expression.Lambda<Func<TEntity, bool>>
                    (
                    Expression.Equal(
                        Expression.Property(
                             itemParameter,
                             typeof(TEntity).GetPrimaryKey().Name
                             ),
                        Expression.Constant(id)
                        ),
                    new[] { itemParameter }
                    );
                return dataConnection.GetTable<TEntity>().Where(whereExpression).FirstOrDefault();
            }
        }

        public TEntity GetById(object id, long clientId)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                var itemParameter = Expression.Parameter(typeof(TEntity), "item");
                var whereExpression = Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(
                        Expression.Property(
                             itemParameter,
                             typeof(TEntity).GetPrimaryKey().Name
                             ),
                        Expression.Constant(id)
                        ),
                    new[] { itemParameter }
                    );
                var whereExpressionClientId = Expression.Lambda<Func<TEntity, bool>>(
                       Expression.Equal(
                           Expression.Property(
                                itemParameter,
                                 typeof(TEntity).GetProperty("ClientId").Name
                                ),
                           Expression.Constant(clientId)
                           ),
                       new[] { itemParameter }
                       );
                return dataConnection.GetTable<TEntity>().Where(whereExpression).Where(whereExpressionClientId).FirstOrDefault();
            }
        }

        public void InsertOnly(TEntity entity)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                dataConnection.Insert(entity);
            }
        }

        public TEntity Insert(TEntity entity)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                //InsertWithInt32Identity
                PropertyInfo idPrimaryKey = typeof(TEntity).GetIdentityPrimaryKey();
                object autoAssignedId = null;
                if (idPrimaryKey.PropertyType == typeof(int))
                    autoAssignedId = dataConnection.InsertWithInt32Identity(entity);
                else if (idPrimaryKey.PropertyType == typeof(long))
                    autoAssignedId = dataConnection.InsertWithInt64Identity(entity);
                else if (idPrimaryKey.PropertyType == typeof(decimal))
                    autoAssignedId = dataConnection.InsertWithDecimalIdentity(entity);
                else
                    throw new ApplicationException(string.Format("Primary key identity must be int, long or decimal for type {0}", typeof(TEntity).Name));

                idPrimaryKey.SetValue(entity, autoAssignedId, null);

                return entity;
            }
        }

        public TId Insert<TId>(TEntity entity) where TId : struct
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                return (TId)dataConnection.InsertWithIdentity(entity);
            }
        }

        public bool Insert(List<TEntity> listOfEntities)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                var bulkCopyResult = dataConnection.BulkCopy<TEntity>(listOfEntities);
                return bulkCopyResult.RowsCopied == listOfEntities.Count;
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                int updatedRows = dataConnection.Update(entity);
                return updatedRows > 0 ? entity : null;
            }
        }

        public TEntity InsertOrUpdate(TEntity entity)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                dataConnection.InsertOrReplace(entity);
                return entity;
            }
        }

        public bool Delete(TEntity entity)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                int deletedRecords = dataConnection.Delete(entity);
                return deletedRecords > 0;
            }
        }

        public bool DeleteById(object id)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                var itemParameter = Expression.Parameter(typeof(TEntity), "item");
                var whereExpression = Expression.Lambda<Func<TEntity, bool>>
                    (
                    Expression.Equal(
                        Expression.Property(
                             itemParameter,
                             typeof(TEntity).GetPrimaryKey().Name
                             ),
                        Expression.Constant(id)
                        ),
                    new[] { itemParameter }
                    );
                int deletedRecords = dataConnection.GetTable<TEntity>().Where(whereExpression).Delete();
                return deletedRecords > 0;
            }
        }

        public bool DeleteById(object id, long clientId)
        {
            using (dataConnection = new PmsDB(providerName, connectionString))
            {
                PropertyInfo propInfo = typeof(TEntity).GetProperty("IsDeleted");
                object value = true;
                if (propInfo != null)
                {
                    var itemParameter = Expression.Parameter(typeof(TEntity), "item");
                    var whereExpressionId = Expression.Lambda<Func<TEntity, bool>>
                       (
                       Expression.Equal(
                           Expression.Property(
                                itemParameter,
                                typeof(TEntity).GetPrimaryKey().Name
                                ),
                           Expression.Constant(id)
                           ),
                       new[] { itemParameter }
                       );
                    var whereExpressionClientId = Expression.Lambda<Func<TEntity, bool>>
                       (
                       Expression.Equal(
                           Expression.Property(
                                itemParameter,
                                 typeof(TEntity).GetProperty("ClientId").Name
                                ),
                           Expression.Constant(clientId)
                           ),
                       new[] { itemParameter }
                       );
                    int deletedRecords = dataConnection.GetTable<TEntity>()
                                .Where(whereExpressionId)
                                .Where(whereExpressionClientId)
                                .Set(t => Sql.Property<TEntity>(t, propInfo.Name), value)
                                .Update();
                    return deletedRecords > 0;

                }
                else
                {
                    var itemParameter = Expression.Parameter(typeof(TEntity), "item");
                    var whereExpressionId = Expression.Lambda<Func<TEntity, bool>>
                        (
                        Expression.Equal(
                            Expression.Property(
                                 itemParameter,
                                 typeof(TEntity).GetPrimaryKey().Name
                                 ),
                            Expression.Constant(id)
                            ),
                        new[] { itemParameter }
                        );
                    var whereExpressionClientId = Expression.Lambda<Func<TEntity, bool>>
                       (
                       Expression.Equal(
                           Expression.Property(
                                itemParameter,
                                 typeof(TEntity).GetProperty("ClientId").Name
                                ),
                           Expression.Constant(clientId)
                           ),
                       new[] { itemParameter }
                       );
                    int deletedRecords = dataConnection.GetTable<TEntity>()
                                .Where(whereExpressionId)
                                .Where(whereExpressionClientId)
                                .Delete();
                    return deletedRecords > 0;
                }
            }
        }
        public static string EncryptString(string plainText)
        {
            string key = "b14ca5898a4e4133bbce2ea2315a1916";
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }


        public static string DecryptString(string cipherText)
        {
            string key = "b14ca5898a4e4133bbce2ea2315a1916";
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
