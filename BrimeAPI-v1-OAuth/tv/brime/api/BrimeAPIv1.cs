using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient;

namespace BrimeAPIv1OAuth.tv.brime.api {
    public class BrimeAPIv1 {

        NLog.Logger classLog = NLog.LogManager.GetCurrentClassLogger();

        public readonly string ClientID;

        public BrimeAPIv1(string clientID) {
            this.ClientID = clientID;
        }

        private async void SignIn() {
            string redirectURI = string.Format("http://127.0.0.1:7890/");

            var http = new HttpListener();
            http.Prefixes.Add(redirectURI);
            http.Start();

            var options = new OidcClientOptions {
                Authority = "https://auth.brime.tv/",
                ClientId = ClientID,
                Scope = "openid profile offline_access name given_name family_name nickname email email_verified picture created_at identities phone address",
                RedirectUri = redirectURI,
                ProviderInformation = new ProviderInformation() {
                    IssuerName = "https://auth.brime.tv/",
                    AuthorizeEndpoint = "https://auth.brime.tv/authorize",
                    TokenEndpoint = "https://auth.brime.tv/oauth/token",
                    UserInfoEndpoint = "https://auth.brime.tv/userinfo"
                }
            };

            var client = new OidcClient(options);
            var state = await client.PrepareLoginAsync();

            Process.Start(state.StartUrl);

            var context = await http.GetContextAsync();
            var formData = GetRequestPostData(context.Request);

            var response = context.Response;
            string responseString = string.Format("<html><head><meta http-equiv='refresh' content='10;url=https://brime.tv' /></head><body>Please return to the app.</body></html>");
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            await responseOutput.WriteAsync(buffer, 0, buffer.Length);
            responseOutput.Close();

            var result = await client.ProcessResponseAsync(formData, state);

            if (result.IsError) {
                classLog.Error(result.Error);
            } else {
                classLog.Info(() => {
                    string _result = "Claims:";
                    foreach (var claim in result.User.Claims) {
                        _result += string.Format("\n{0}: {1}", claim.Type, claim.Value);
                    }
                    _result += "\n\nAccess Token: " + result.AccessToken;
                    if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                        _result += "\n\nRefresh Token: " + result.RefreshToken;
                    return _result;
                });
            }

            http.Stop();
        }

        public static string GetRequestPostData(HttpListenerRequest request) {
            if (!request.HasEntityBody) return null;

            using (var body = request.InputStream) {
                using (var reader = new StreamReader(body, request.ContentEncoding)) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
