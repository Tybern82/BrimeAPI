#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BrimeAPI.com.brimelive.api {
    public class BrimeAPIResponse {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public JObject Data { get; private set; }
        public List<BrimeAPIError> Errors { get; private set; }

        public string __Notice { get; private set; }

        public BrimeAPIResponse(string json) {
            JObject apiResponse = JObject.Parse(json);
            JObject? data = apiResponse.Value<JObject>("data");
            if (data == null) {
                Logger.Warn("Missing 'data' in JSON response");
                Data = JObject.Parse("{}");
            } else {
                Data = data;
            }
            JArray? apiErrors = apiResponse.Value<JArray>("errors");
            if (apiErrors != null) {
                Errors = new List<BrimeAPIError>(apiErrors.Count);
                foreach (JToken err in apiErrors) {
                    string? eMessage = err.ToObject<string>();
                    if (eMessage == null) {
                        Logger.Error("Unable to decode error in response");
                        throw new BrimeAPIMalformedResponse("Unable to decode error in response");
                    }
                    Errors.Add(BrimeAPIError.lookupError(eMessage));
                }
            } else {
                Errors = new List<BrimeAPIError>();
            }
            Logger.Warn(() => {
                return String.Join<BrimeAPIError>(", ", Errors);
            });
            string? notice = apiResponse.Value<string>("__notice");
            __Notice = (notice == null) ? "" : notice;
            if (!string.IsNullOrWhiteSpace(__Notice)) Logger.Info(() => { return "API-NOTICE: " + __Notice; });
        }
    }
}
