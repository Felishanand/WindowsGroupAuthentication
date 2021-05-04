using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application1
{
    public class HttpClientHelper
    {
        private static HttpClientHelper _Instance;
        private string _CertificateThumbprint;
        private bool isSecurityDisabled = false;
        private HttpClientHelper()
        {
            //            
            //    isSecurityDisabled = true;            
        }

        public static HttpClientHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new HttpClientHelper();
                }

                return _Instance;
            }
        }

        public HttpResponseMessage PostAsync(string baseUri, string postUri, object postContent/*, string accessToken = null*/)
        {
            if (!baseUri.EndsWith("/")) { baseUri += "/"; }
            postUri = postUri.TrimStart('/');


            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, error) =>
            {
                //   if (isSecurityDisabled) { return true; }
                return true;
                if (string.IsNullOrEmpty(_CertificateThumbprint))
                {
                    //ToDo: Commented For Resolving Build Issue.
                    //_CertificateThumbprint = OJCA.Security.CertificateHelper.Certificate.GetCertificateThumbprint(OJCAConfigReader.Instance.Configuration.Security.CertificateName);
                }
                return cert.GetCertHashString() == _CertificateThumbprint;
            };

            using (var handler = new HttpClientHandler() { UseDefaultCredentials = true })
            {
                // handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                using (var client = new HttpClient(handler))
                {
                    //if (!string.IsNullOrEmpty(accessToken))
                    //{
                    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", $"{accessToken}");
                    //}

                    client.BaseAddress = new Uri(baseUri);

                    var jsonRequest = JsonConvert.SerializeObject(postContent);
                    var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    var responseTask = client.PostAsync(postUri, stringContent);
                    responseTask.Wait();

                    var result = responseTask.Result;

                    return result;
                }
            }
        }

        public T PostAsync<T>(string BaseUri, string PostUri, object PostContent)
        {
            var result = PostAsync(BaseUri, PostUri, PostContent);
            result.EnsureSuccessStatusCode();
            return DeserializeHttpResponse<T>(result);
        }


        public HttpResponseMessage GetAsync(string baseUri, string getUri/*, string accessToken = null*/)
        {
            try
            {
                if (!baseUri.EndsWith("/")) { baseUri += "/"; }
                getUri = getUri.TrimStart('/');


                ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, error) =>
                {
                    return true;
                    if (isSecurityDisabled) { return true; }

                    if (string.IsNullOrEmpty(_CertificateThumbprint))
                    {
                        //ToDo: Commented For Resolving Build Issue.
                        //_CertificateThumbprint = OJCA.Security.CertificateHelper.Certificate.GetCertificateThumbprint(OJCAConfigReader.Instance.Configuration.Security.CertificateName);
                    }
                    return cert.GetCertHashString() == _CertificateThumbprint;
                };

                using (var handler = new HttpClientHandler() { UseDefaultCredentials = true })
                {
                    handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                    //handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                    using (var client = new HttpClient(handler))
                    {
                        //if (!string.IsNullOrEmpty(accessToken))
                        //{
                        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", $"{accessToken}");
                        //}

                        client.BaseAddress = new Uri(baseUri);

                        var responseTask = client.GetAsync(getUri);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        return result;
                    }
                }
            }
            catch (Exception exc)
            {
                //ToDo: Commented For Resolving Build Issue.
                //OJCALogger.Log.Error($"Data fetch failed at {baseUri}/{getUri}", exc);
                throw (exc);
            }
        }


        public HttpResponseMessage DeleteAsync(string baseUri, string deleteUri, object deleteContent)
        {
            if (!baseUri.EndsWith("/")) { baseUri += "/"; }
            deleteUri = deleteUri.TrimStart('/');


            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, error) =>
            {
                if (isSecurityDisabled) { return true; }

                if (string.IsNullOrEmpty(_CertificateThumbprint))
                {
                    //ToDo: Commented For Resolving Build Issue.
                    //_CertificateThumbprint = OJCA.Security.CertificateHelper.Certificate.GetCertificateThumbprint(OJCAConfigReader.Instance.Configuration.Security.CertificateName);
                }
                return cert.GetCertHashString() == _CertificateThumbprint;
            };

            using (var handler = new HttpClientHandler() { UseDefaultCredentials = true })
            {
                // handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(baseUri);

                    var jsonRequest = JsonConvert.SerializeObject(deleteContent);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(client.BaseAddress, deleteUri),
                        Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
                    };

                    var responseTask = client.SendAsync(request);
                    responseTask.Wait();

                    var result = responseTask.Result;

                    return result;
                }
            }
        }


        public T GetAsync<T>(string BaseUri, string GetUri)
        {
            var result = GetAsync(BaseUri, GetUri);
            result.EnsureSuccessStatusCode();
            return DeserializeHttpResponse<T>(result);
        }

        public T DeserializeHttpResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            var jsonResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }
    }
}
