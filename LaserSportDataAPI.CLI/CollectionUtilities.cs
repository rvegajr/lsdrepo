using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserSportDataAPI.CLI
{
    class CollectionUtilities<T, TEntity>
    {
        public Dictionary<T,TEntity> ListToDictionary(List<TEntity> listToIndex, string PropertyToKey) {
            var ret = new Dictionary<T, TEntity>();
            foreach (TEntity ent in listToIndex)
            {
                object obj = ent.GetType().GetProperty(PropertyToKey).GetValue(ent, null);
                if (obj != null)
                {
                    ret.Add((T)obj, ent);
                }
            }
            return ret;
        }
    }
}
