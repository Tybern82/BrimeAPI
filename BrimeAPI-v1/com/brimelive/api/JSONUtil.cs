#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api {
    /// <summary>
    /// Used to identify classes which have implemented the toJSON method which should be used
    /// in place of the default <c>JsonConvert.ToString(object)</c> method.
    /// </summary>
    public interface JSONConvertable {
        /// <summary>
        /// Convert this object into a JSON representation. If object takes a JToken parameter to a constructor, 
        /// the return from this method should be able to be loaded using this constructor to produce an 
        /// equivalent object.
        /// </summary>
        /// <returns>JSON equivalent to this object</returns>
        public string toJSON();
    }

    /// <summary>
    /// Helper class for performing conversions to JSON. 
    /// </summary>
    public static class JSONUtil {

        /// <summary>
        /// Helper method to identify whether a JToken contains an entry for a specific named value.
        /// </summary>
        /// <param name="token"><c>JToken</c> to check</param>
        /// <param name="value">name of entry to look for</param>
        /// <returns>true if the <c>JToken</c> contains an entry of this name, false otherwise</returns>
        public static bool HasValue(this JToken token, string value) {
            return (token[value] != null);
        }

        /// <summary>
        /// Label the given value with provided name
        /// </summary>
        /// <param name="name">name to use for label</param>
        /// <param name="value">JSON value to label</param>
        /// <returns></returns>
        private static string makeEntry(string name, string value) {
            return "\"" + name + "\": " + value;
        }

        /// <summary>
        /// Convert parameter to JSON and label with given name
        /// </summary>
        /// <param name="o">value to convert to JSON</param>
        /// <param name="name">name to use for label</param>
        /// <returns>"name": {value:JSON}</returns>
        public static string toJSON(this object o, string name) {
            return makeEntry(name, o.toJSON());
        }

        /// <summary>
        /// Convert parameter to JSON - use toJSON method if defined
        /// </summary>
        /// <param name="o">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON(this object o) {
            if (o is JSONConvertable) {
                return (o as JSONConvertable)?.toJSON() ?? "";
            } else {
                return JsonConvert.ToString(o);
            }
        }

        /// <summary>
        /// Convert parameter to JSON - calls the toJSON method
        /// </summary>
        /// <param name="j">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON(this JSONConvertable j) {
            return j.toJSON();
        }

        /// <summary>
        /// Convert parameter to JSON - uses JsonConvert.ToString
        /// </summary>
        /// <param name="i">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON(this int i) {
            return JsonConvert.ToString(i);
        }

        /// <summary>
        /// Convert parameter to JSON - uses JsonConvert.ToString
        /// </summary>
        /// <param name="l">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON(this long l) {
            return JsonConvert.ToString(l);
        }

        /// <summary>
        /// Convert parameter to JSON - uses JsonConvert.ToString
        /// </summary>
        /// <param name="b">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON(this bool b) {
            return JsonConvert.ToString(b);
        }

        /// <summary>
        /// Convert parameter to JSON - uses JsonConvert.ToString
        /// </summary>
        /// <param name="s">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON(this string s) {
            return JsonConvert.ToString(s);
        }

        /// <summary>
        /// Convert parameter to JSON - uses JsonConvert.ToString on the AbsoluteUri
        /// </summary>
        /// <param name="uri">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON(this Uri uri) {
            return uri.AbsoluteUri.toJSON();
        }

        /// <summary>
        /// Convert parameter to JSON - constructs a JSON array
        /// </summary>
        /// <param name="l">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON<T>(this List<T> l) {
            string _result = "[";
            if (l.Count > 0) {
                _result += l[0]?.toJSON();
                for (int i = 1; i < l.Count; i++) {
                    _result += ", " + l[i]?.toJSON();
                }
            }
            _result += "]";
            return _result;
        }

        /// <summary>
        /// Convert parameter to JSON - constructs a JSON array
        /// </summary>
        /// <param name="l">parameter to convert</param>
        /// <param name="name">name to use to label JSON</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON<T>(this List<T> l, string name) {
            return makeEntry(name, toJSON<T>(l));
        }

        /// <summary>
        /// Convert parameter to JSON - constructs a JSON array
        /// </summary>
        /// <param name="l">parameter to convert</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON<T>(this T[] l) {
            string _result = "[";
            if (l.Length > 0) {
                _result += l[0]?.toJSON();
                for (int i = 1; i < l.Length; i++)
                    _result += ", " + l[i]?.toJSON();
            }
            _result += "]";
            return _result;
        }

        /// <summary>
        /// Convert parameter to JSON - constructs a JSON array
        /// </summary>
        /// <param name="l">parameter to convert</param>
        /// <param name="name">name to use to label JSON</param>
        /// <returns>JSON value of parameter</returns>
        public static string toJSON<T>(this T[] l, string name) {
            return makeEntry(name, toJSON<T>(l));
        }
    }
}
