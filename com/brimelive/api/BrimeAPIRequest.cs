using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BrimeAPI.com.brimelive.api {

    public enum BrimeAPIEndpoint {
        PRODUCTION, STAGING, SANDBOX
    }

    public abstract class BrimeAPIRequest<ResponseType> {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected delegate string[] GetRequestParameters();

        public BrimeAPIEndpoint APIEndpoint { get; private set; } = BrimeAPIEndpoint.STAGING;   // TODO: Update default to PRODUCTION

        public string ClientID { get; private set; } = "";

        public abstract ResponseType getResponse();

        protected string RequestFormat { get; set; }
        protected GetRequestParameters RequestParameters { get; set; } = (() => { return new string[0]; });

        public BrimeAPIRequest(string requestFormat) {
            this.RequestFormat = requestFormat;
        }

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
            _result += "?client_id=" + ClientID;
            return _result;
        }

        protected BrimeAPIResponse doRequest() {
            string request = composeRequest(RequestFormat, RequestParameters.Invoke());
            // TODO: Update to handle rate limiting, etc
            // May pass off to central request handler for async processing
            Logger.Debug(() => { return "REQUEST: " + request; });
            WebResponse response = WebRequest.Create(request).GetResponse();
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Logger.Debug(() => { return "RESPONSE: " + response; });
            return new BrimeAPIResponse(json);
        }
    }
}
