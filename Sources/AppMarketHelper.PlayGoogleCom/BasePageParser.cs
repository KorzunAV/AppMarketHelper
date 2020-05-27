using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AppMarketHelper.PlayGoogleCom
{
    public abstract class BasePageParser
    {
        protected static readonly Regex InitDataCallbackRegex = new Regex(@"AF_initDataCallback[\s\S]*?<\/script");
        protected static readonly Regex InitDataCallbackKeyRegex = new Regex(@"'(ds:.*?)'");
        protected static readonly Regex InitDataCallbackValueRegex = new Regex(@"return ([\s\S]*?)}}\);<\/");
        protected static readonly Regex NotNumbersRegex = new Regex(@"[^\d]");


        protected T TryGetValue<T>(JArray array, params int[] ids)
        {
            var ch = array;
            for (var i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                if (ch.HasValues && ch.Count > id)
                {
                    var bf = ch[id];
                    if (bf.Type == JTokenType.Array)
                    {
                        ch = (JArray)bf;
                    }
                    else if (i == ids.Length - 1)
                    {
                        return bf.Value<T>();
                    }
                    else
                    {
                        return default(T);
                    }
                }
                else
                {
                    return default(T);
                }
            }

            return ch.Value<T>();
        }

        protected IEnumerable<T> TryGetValues<T>(JArray array, params int[] ids)
        {
            if (array == null)
                yield break;

            foreach (var item in array)
            {
                if (item.Type == JTokenType.Null)
                    continue;

                if (item.Type != JTokenType.Array)
                {
                    if (ids.Length == 0)
                    {
                        yield return item.Value<T>();
                    }
                    else
                    {
                        break;
                    }
                }

                var ch = (JArray)item;
                for (var i = 0; i < ids.Length; i++)
                {
                    var id = ids[i];
                    if (ch.HasValues && ch.Count >= id)
                    {
                        var bf = ch[id];
                        if (bf.Type == JTokenType.Array)
                        {
                            ch = (JArray)ch[id];
                        }
                        else if (i == ids.Length - 1)
                        {
                            yield return bf.Value<T>();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}