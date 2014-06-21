using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsVenture.Core.Extensions
{
    public static class DbEntityEntryExtensions
    {
        public static bool IsAnyFieldChanged<TEntity>(this DbEntityEntry<TEntity> entry, TEntity entity, IEnumerable<string> ignoredProperties = null)
            where TEntity : class
        {
            return entry.GetChangedFields(entity, ignoredProperties).Any();
        }

        public static Dictionary<string, object[]> GetChangedFields<TEntity>(this DbEntityEntry<TEntity> entry, TEntity entity, IEnumerable<string> ignoredProperties)
            where TEntity : class
        {
            var fields = new Dictionary<string, object[]>();

            var originalValuesColl = entry.OriginalValues;
            var originalValues = originalValuesColl
                                    .PropertyNames
                                    .Where(x => !ignoredProperties.Contains(x))
                                    .ToDictionary(
                                        x => x,
                                        x => originalValuesColl[x]
                                    );

            foreach (var originalValue in originalValues)
            {
                var prop = typeof(TEntity).GetProperty(originalValue.Key);
                var propType = prop.PropertyType;

                //Get properties values
                var valueBefore = originalValue.Value;
                var valueAfter = prop.GetGetMethod().Invoke(entity, Enumerable.Empty<object>().ToArray());

                //If values are different, add them to object with differences
                if (!AreEqualValues(valueBefore, valueAfter))
                {
                    fields.Add(prop.Name, new object[] {
					    valueBefore,
					    valueAfter
				    });
                }
            }
            return fields;
        }

        private static bool AreEqualValues(object valueBefore, object valueAfter)
        {
            return valueBefore == null && valueAfter == null
                || valueBefore != null && valueBefore.Equals(valueAfter)
                || valueAfter != null && valueAfter.Equals(valueBefore)
                || AreEqualDates(valueBefore, valueAfter);
        }

        /// <summary>
        /// Returns true if params are equal DateTimes (not null) ignoring Kind property if any is Unspecified
        /// </summary>
        private static bool AreEqualDates(object valueBefore, object valueAfter)
        {
            var dateBefore = valueBefore as DateTime?;
            var dateAfter = valueAfter as DateTime?;
            return dateBefore != null 
                && dateAfter != null 
                && (dateBefore.Value.Kind == dateAfter.Value.Kind 
                    || dateBefore.Value.Kind == DateTimeKind.Unspecified
                    || dateAfter.Value.Kind == DateTimeKind.Unspecified)
                && dateBefore.Value.ToString() == dateAfter.Value.ToString();
        }
    }
}
