using LinqToDB.Mapping;
using System;
using System.Linq;
using System.Reflection;

namespace ds.pms.dal.EntityHelper
{
    public static class GenericPropertyInfo
    {
        public static PropertyInfo GetPrimaryKey(this Type entityType)
        {
            var primaryKeyProp = entityType.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute)));
            if (primaryKeyProp != null && primaryKeyProp.Any() && primaryKeyProp.Count() == 1)
                return primaryKeyProp.First();
            else if (primaryKeyProp != null && primaryKeyProp.Any() && primaryKeyProp.Count() == 1)
                throw new ApplicationException(string.Format("More than one column is primary key for type {0}", entityType.Name));
            else
                throw new ApplicationException(string.Format("No primary key defined for type {0}", entityType.Name));
        }

        public static PropertyInfo GetIdentityPrimaryKey(this Type entityType)
        {
            var identityPrimaryKeyProp = entityType.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute))
                && Attribute.IsDefined(prop, typeof(IdentityAttribute)));

            if (identityPrimaryKeyProp != null && identityPrimaryKeyProp.Any() && identityPrimaryKeyProp.Count() == 1)
                return identityPrimaryKeyProp.First();

            throw new ApplicationException(string.Format("No single identity primary key defined for type {0}", entityType.Name));
        }
        public static object GetPrimaryKeyValue<TEntity>(TEntity entity)
        {
            Type entityType = entity.GetType();
            var primaryKeyProp = entityType.GetPrimaryKey();
            if (primaryKeyProp != null)
                return primaryKeyProp.GetValue(entity, null);
            return null;
        }
    }
}
