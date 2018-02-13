#r "Newtonsoft.Json"

using System;
using System.Net;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    var httpStatus = HttpStatusCode.BadRequest;
    var messages = new List<string>();

    // Get request body
    string requestContent = await req.Content.ReadAsStringAsync();
    log.Info(requestContent);
    dynamic requestObject = JsonConvert.DeserializeObject(requestContent);
    if(String.IsNullOrWhiteSpace(requestObject?.displayName?.ToString()))
    {
        messages.Add("Display name is required.");
    }
    if (requestObject?.email?.ToString().EndsWith("@tangerineglobal.com") != true)
    {
        messages.Add("Invalid e-mail address.");
    }

    var response = messages.Any() ?
        req.CreateResponse(
            httpStatus, 
            new ResponseContent
            {
                version = "1.0.0",
                status = (int)httpStatus,
                userMessage = String.Join(Environment.NewLine, messages),
            })
        : req.CreateResponse(
            HttpStatusCode.OK
        );
    
    log.Info(await response.Content.ReadAsStringAsync());

    return response;
}

public class ResponseContent
{
    public string version { get; set; }

    public int status { get; set; }

    public string userMessage { get; set; }
}