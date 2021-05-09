#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BrimeAPI.com.brimelive.api {
    public class JSONUtil {

        public static string ToString(string[] data) {
            StringBuilder _result = new StringBuilder();
            _result.Append("[");
            bool isFirst = true;
            foreach (string s in data) {
                if (!isFirst) _result.Append(", ");
                _result.Append(JsonConvert.ToString(s));
                isFirst = false;
            }
            _result.Append("]");
            return _result.ToString();
        }

        public static string ToString(List<string> data) {
            StringBuilder _result = new StringBuilder();
            _result.Append("[");
            if (data.Count > 0) _result.Append(JsonConvert.ToString(data[0]));
            for (int i = 1; i < data.Count; i++)
                _result.Append(",").Append(JsonConvert.ToString(data[i]));
            _result.Append("]");
            return _result.ToString();
        }
    }
}
