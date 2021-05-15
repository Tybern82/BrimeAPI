﻿#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BrimeAPI.com.brimelive.api {

    public enum BrimeAPIEndpoint {
        PRODUCTION, STAGING, SANDBOX
    }

    public enum BrimeRequestMode {
        GET, POST
    }

    public abstract class BrimeAPIRequest<ResponseType> {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected delegate string[] GetRequestParameters();

        public BrimeAPIEndpoint APIEndpoint { get; private set; } = BrimeAPIEndpoint.STAGING;   // TODO: Update default to PRODUCTION

        public static string ClientID { get; set; } = "";
        public bool RequiresSpecialAccess { get; private set; } = false;

        public abstract ResponseType getResponse();

        protected string RequestFormat { get; set; }
        protected GetRequestParameters RequestParameters { get; set; } = (() => { return new string[0]; });

        protected BrimeRequestMode RequestMode { get; set; } = BrimeRequestMode.GET;

        protected string PostBody { get; set; } = "";

        public BrimeAPIRequest(string requestFormat, bool requiresSpecialAccess) {
            this.RequestFormat = requestFormat;
            this.RequiresSpecialAccess = requiresSpecialAccess;
        }

        public BrimeAPIRequest(string requestFormat) : this(requestFormat, false) {}

        protected string getAPIEndpoint() {
            switch (APIEndpoint) {
                case BrimeAPIEndpoint.PRODUCTION:   return "https://api.brimelive.com";
                case BrimeAPIEndpoint.STAGING:      return "https://api-staging.brimelive.com/v1";
                case BrimeAPIEndpoint.SANDBOX:
                default:                            return "https://api-sandbox.brimelive.com";
            }
        }

        protected string composeRequest(string requestFormat, string[] requestParams) {
            string _result = getAPIEndpoint();
            _result += string.Format(requestFormat, requestParams); 
            if (_result.Contains("?")) {
                // already has query parameters, append extra query parameter
                _result += "&client_id=" + ClientID;
            } else {
                // no existing parameters, just add as single
                _result += "?client_id=" + ClientID;
            }
            return _result;
        }

        protected BrimeAPIResponse doRequest() {
            string request = composeRequest(RequestFormat, RequestParameters.Invoke());
            // TODO: Update to handle rate limiting, etc
            // May pass off to central request handler for async processing
            Logger.Debug(() => { return "REQUEST: " + request; });
            if (RequiresSpecialAccess) Logger.Warn("Request requires SPECIAL ACCESS enabled for the Client-ID.");
            WebRequest req = WebRequest.Create(request);
            if (RequestMode == BrimeRequestMode.POST) {
                byte[] data = Encoding.UTF8.GetBytes(PostBody);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = data.Length;

                using (var stream = req.GetRequestStream()) {
                    stream.Write(data, 0, data.Length);
                }
            }
            WebResponse response = req.GetResponse();
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Logger.Debug(() => { return "RESPONSE: " + response; });
            return new BrimeAPIResponse(json);
        }
    }
}
