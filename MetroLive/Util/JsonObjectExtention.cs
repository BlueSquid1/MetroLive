using System;
using System.Text.RegularExpressions;
using ServiceStack.Text;
using System.Linq;

namespace MetroLive.Util
{
	public static class JsonObjectExtention
	{
		public static T GetByBPath<T>(this JsonObject jsonObj, string path)
		{
            try
            {
                string[] entries = path.Split('/');
                JsonObject tempObj = jsonObj;
                for (int i = 0; i < entries.Length - 1; i++)
                {
                    string curEntry = entries[i];
                    tempObj = ProccessEntry<JsonObject>(curEntry, tempObj);
                }

                //on the last one return the right type
                string lastEntry = entries.Last();
                return ProccessEntry<T>(lastEntry, tempObj);
            }
            catch
            {
                //something went wrong
                return default(T);
            }
		}


        private static T ProccessEntry<T>(string objectEntry, JsonObject jsonObj)
        {
			if (jsonObj == null)
			{
				throw new NullReferenceException("json object is null.");
			}

			Match arrayMatch = Regex.Match(objectEntry, "\\[[0-9]+\\]$");
			if (arrayMatch.Success)
			{
				//array object
				string[] subEntries = Regex.Split(objectEntry, @"\[|\]");
                if (subEntries.Length != 3 | subEntries?.ElementAt(2) != "")
				{
                    //parser error
                    throw new ArgumentException("Entry=" + objectEntry + " does not match the format ENTRY[INDEX].");
				}
				int index = int.Parse(subEntries[1]);
				string entry = subEntries[0];

                if (jsonObj.ContainsKey(entry) == false)
                {
                    throw new MissingFieldException("entry=" + entry + " does not exist in json.");
                }

				T[] jsonObjects = jsonObj.GetArray<T>(entry);
				return jsonObjects[index];

			}
			else
			{
                //single entry
                if(jsonObj.ContainsKey(objectEntry) == false)
                {
                    throw new MissingFieldException("entry=" + objectEntry + " does not exist in json.");
                }
				return jsonObj.Get<T>(objectEntry);
			}
        }
	}
}
