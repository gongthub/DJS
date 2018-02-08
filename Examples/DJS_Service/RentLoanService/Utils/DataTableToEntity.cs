using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanService.Utils
{
    public class DataTableToEntity
    {

        //T entity = (T)Activator.CreateInstance(typeof(T));

        public static T FirstEntity<T>(DataTable dt) where T : class
        {
            if (dt.Rows.Count > 0)
            {
                DataRow dw = dt.Rows[0];
                return (T)ToEntity(dw, typeof(T));
            }
            return null;
        }

        public static T LastEntity<T>(DataTable dt) where T : class
        {
            if (dt.Rows.Count > 0)
            {
                DataRow dw = dt.Rows[dt.Rows.Count - 1];
                return (T)ToEntity(dw, typeof(T));
            }
            return null;
        }

        public static List<T> ListEntity<T>(DataTable dt) where T : class
        {
            List<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dw = dt.Rows[i];
                list.Add((T)ToEntity(dw, typeof(T)));
            }
            return list;
        }

        private static object ToEntity(DataRow adaptedRow, Type entityType)
        {
            if (entityType == null || adaptedRow == null)
            {
                return null;
            }

            object entity = Activator.CreateInstance(entityType);
            CopyToEntity(entity, adaptedRow);

            return entity;
        }

        private static T ToEntity<T>(DataRow adaptedRow, T value) where T : new()
        {
            T item = new T();
            if (value == null || adaptedRow == null)
            {
                return item;
            }

            item = Activator.CreateInstance<T>();
            CopyToEntity(item, adaptedRow);

            return item;
        }

        private static void CopyToEntity(object entity, DataRow adaptedRow)
        {
            if (entity == null || adaptedRow == null)
            {
                return;
            }
            PropertyInfo[] propertyInfos = entity.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (!CanSetPropertyValue(propertyInfo, adaptedRow))
                {
                    continue;
                }

                try
                {
                    if (adaptedRow[propertyInfo.Name] is DBNull)
                    {
                        propertyInfo.SetValue(entity, null, null);
                        continue;
                    }
                    SetPropertyValue(entity, adaptedRow, propertyInfo);
                }
                finally
                {

                }
            }
        }

        private static bool CanSetPropertyValue(PropertyInfo propertyInfo, DataRow adaptedRow)
        {
            if (!propertyInfo.CanWrite)
            {
                return false;
            }

            if (!adaptedRow.Table.Columns.Contains(propertyInfo.Name))
            {
                return false;
            }

            return true;
        }

        private static void SetPropertyValue(object entity, DataRow adaptedRow, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(DateTime?) ||
                propertyInfo.PropertyType == typeof(DateTime))
            {
                DateTime date = DateTime.MaxValue;
                DateTime.TryParse(adaptedRow[propertyInfo.Name].ToString(),
                    CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                propertyInfo.SetValue(entity, date, null);
            }
            else
            {
                propertyInfo.SetValue(entity, adaptedRow[propertyInfo.Name], null);
            }
        }
    }
}
