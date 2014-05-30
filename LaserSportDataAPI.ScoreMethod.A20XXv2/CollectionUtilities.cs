using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
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

public static class ExtensionMethods
{
    public static void CopyTo<T>(this T source, T dest)
    {
        var plist = from prop in typeof(T).GetProperties() where prop.CanRead && prop.CanWrite select prop;

        foreach (PropertyInfo prop in plist)
        {
            prop.SetValue(dest, prop.GetValue(source, null), null);
        }
    }
}

