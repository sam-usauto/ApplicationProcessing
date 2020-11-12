using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace Common.Helper
{
    public class JsonConvertion
    {

        public static string ObjectToJson<T>(T Obj, bool Indented = true)
        {
            try
            {
                var json = "";
                if (Indented)
                {
                    json = JsonConvert.SerializeObject(Obj, Formatting.Indented);
                }
                else
                {
                    json = JsonConvert.SerializeObject(Obj, Formatting.None);
                }
                return json;
            }
            catch (JsonException ex)
            {
                throw ex;
            }
        }

    }
}
