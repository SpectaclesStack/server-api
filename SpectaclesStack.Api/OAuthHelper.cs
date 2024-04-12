namespace SpectacularOauth
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Text.Json;
    using Microsoft.AspNetCore.Http;

    // This code must go to the api side
    static class OauthHelper
    {
        private static string client_id = "cddf9a8bc7feeff74c35";
        public static async Task<bool> Autheenticate(string accessToken)
        {
            string apiUrl = $"https://api.github.com/user?client_id={client_id}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "C# HttpClient");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(responseBody);
                    var data = jsonDocument.RootElement;
                    string login = data.GetProperty("login").GetString() ?? "";
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string GetAccessToken(HttpContext httpContext)
        {
            IDictionary<string, string> headers = GetRequestHeaders(httpContext);
            if (!headers.ContainsKey("Authorization"))
                return null;

            return headers["Authorization"].ToString()["Bearer ".Length..].Trim();
        }

        private static IDictionary<string, string> GetRequestHeaders(HttpContext httpContext)
        {
            IDictionary<string, string> headers = new Dictionary<string, string>();

            foreach (var (key, value) in httpContext.Request.Headers)
            {
                headers[key] = value.ToString();
            }

            return headers;
        }
    }

}
