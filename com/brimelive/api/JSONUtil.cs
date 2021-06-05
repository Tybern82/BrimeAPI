#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api {

    /// <summary>
    /// Helper class for performing conversions to JSON. 
    /// </summary>
    public static class JSONUtil {

        /// <summary>
        /// Convert the given array of <c>string</c> items into a JSON array element. Ensures that the items are correctly
        /// converted to JSON strings as they are included.
        /// </summary>
        /// <param name="data">array of items to convert to JSON array of strings</param>
        /// <returns>JSON array of provided string entries</returns>
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

        /// <summary>
        /// Convert the given list of <c>string</c> items into JSON array element. <br/>
        /// See: <see cref="JSONUtil.ToString(string[])"/>
        /// </summary>
        /// <param name="data">list of items to convert to JSON array of strings</param>
        /// <returns>JSON array of provided string entries</returns>
        public static string ToString(List<string> data) {
            StringBuilder _result = new StringBuilder();
            _result.Append("[");
            if (data.Count > 0) _result.Append(JsonConvert.ToString(data[0]));
            for (int i = 1; i < data.Count; i++)
                _result.Append(",").Append(JsonConvert.ToString(data[i]));
            _result.Append("]");
            return _result.ToString();
        }

        /// <summary>
        /// Convert the given item into a new JSON entry. Assumes the value is already a valid JSON item.
        /// </summary>
        /// <param name="value">JSON body for the entry</param>
        /// <param name="name">name for the entry</param>
        /// <returns>"name": value</returns>
        public static string ToJSONEntry(this string value, string name) {
            return "\"" + name + "\": " + value;
        }

        /// <summary>
        /// Collects the given set of items into a new composite entity. Assumes the items in the list are
        /// already valid JSON entries.
        /// </summary>
        /// <param name="items">array of JSON items</param>
        /// <returns>{ item[0], item[1],... item[n] }</returns>
        public static string ToJSONEntry(this string[] items) {
            StringBuilder _result = new StringBuilder();
            _result.Append("{ ");
            if (items.Length > 0) _result.Append(items[0]);
            for (int i = 1; i < items.Length; i++)
                _result.Append(", ").Append(items[i]);
            _result.Append(" }");
            return _result.ToString();
        }

        /// <summary>
        /// Collects the given set of items into a new composite entity. Assumes the items in the list are
        /// already valid JSON entries.
        /// </summary>
        /// <param name="items">list of JSON items</param>
        /// <returns>{ item[0], item[1],... item[n] }</returns>
        public static string TOJSONEntry(this List<string> items) {
            StringBuilder _result = new StringBuilder();
            _result.Append("{ ");
            if (items.Count > 0) _result.Append(items[0]);
            for (int i = 1; i < items.Count; i++)
                _result.Append(", ").Append(items[i]);
            _result.Append(" }");
            return _result.ToString();
        }

        /// <summary>
        /// Collect given set of items into an array. Assumes the items in the array are already valid JSON entries.
        /// </summary>
        /// <param name="items">array of JSON items</param>
        /// <returns>[ item[0], item[1],... item[n] ]</returns>
        public static string MakeJSONArray(this string[] items) {
            StringBuilder _result = new StringBuilder();
            _result.Append("[");
            if (items.Length > 0) _result.Append(items[0]);
            for (int i = 1; i < items.Length; i++) {
                _result.Append(", ").Append(items[i]);
            }
            _result.Append("]");
            return _result.ToString();
        }

        /// <summary>
        /// Convert the given <c>bool</c> value to its JSON equivalent
        /// </summary>
        /// <param name="b">value to process</param>
        /// <returns>JSON equivalent of value</returns>
        public static string ToJSONString(this bool b) {
            return JsonConvert.ToString(b);
        }


        /// <summary>
        /// Convert the given <c>int</c> value to its JSON equivalent
        /// </summary>
        /// <param name="b">value to process</param>
        /// <returns>JSON equivalent of value</returns>
        public static string ToJSONString(this int i) {
            return JsonConvert.ToString(i);
        }


        /// <summary>
        /// Convert the given <c>object</c> value to its JSON equivalent
        /// </summary>
        /// <param name="b">value to process</param>
        /// <returns>JSON equivalent of value</returns>
        public static string ToJSONString(this object? o) {
            return JsonConvert.ToString(o);
        }


        /// <summary>
        /// Convert the given <c>Uri</c> value to its JSON equivalent
        /// </summary>
        /// <param name="b">value to process</param>
        /// <returns>JSON equivalent of value</returns>
        public static string ToJSONString(this Uri uri) {
            return uri.AbsoluteUri.ToJSONString();
        }

        /// <summary>
        /// Wrapper to <c>ToString(string[])</c> - will convert calls to this over to this method then 
        /// move body to this method and remove. <br />
        /// See: <see cref="JSONUtil.ToString(string[])"/>
        /// </summary>
        /// <param name="data">array of items to convert to JSON array of strings</param>
        /// <returns>[ "data[0]", "data[1]",... "data[n]" ]</returns>
        public static string ToJSONString(this string[] data) {
            return ToString(data);
        }

        /// <summary>
        /// Wrapper to <c>ToString(List{string})</c> - will convert calls to this over to this method then 
        /// move body to this method and remove. <br />
        /// See: <see cref="JSONUtil.ToString(List{string})"/>
        /// </summary>
        /// <param name="data">array of items to convert to JSON array of strings</param>
        /// <returns>[ "data[0]", "data[1]",... "data[n]" ]</returns>
        public static string ToJSONString(this List<string> data) {
            return ToString(data);
        }

        /// <summary>
        /// Helper method to identify whether a JToken contains an entry for a specific named value.
        /// </summary>
        /// <param name="token"><c>JToken</c> to check</param>
        /// <param name="value">name of entry to look for</param>
        /// <returns>true if the <c>JToken</c> contains an entry of this name, false otherwise</returns>
        public static bool HasValue(this JToken token, string value) {
            return (token[value] != null);
        }
    }
}
