#r "Newtonsoft.Json"

using System;
using System.Net;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

public static async Task<object> Run(HttpRequestMessage request, TraceWriter log)
{
    log.Info($"{DateTime.Now} Webhook was triggered!");

    string requestContentAsString = await request.Content.ReadAsStringAsync();
    log.Info(requestContentAsString);
    dynamic requestContentAsJObject = JsonConvert.DeserializeObject(requestContentAsString);

    if (requestContentAsJObject.displayName == null)
    {
        return request.CreateResponse(HttpStatusCode.BadRequest);
    }

    var displayName = ((string) requestContentAsJObject.displayName).ToLower();

    if (displayName == "mcvinny" || displayName == "msgates123" || displayName == "revcottonmarcus")
    {
        return request.CreateResponse<ResponseContent>(
            HttpStatusCode.BadRequest,
            new ResponseContent
            {
                version = "1.0.0",
                status = (int) HttpStatusCode.Conflict,
                userMessage = $"{displayName} is not valid."
            },
            new JsonMediaTypeFormatter(),
            "application/json");
    }

    return request.CreateResponse(HttpStatusCode.OK);
}

public class ResponseContent
{
    public string version { get; set; }

    public int status { get; set; }

    public string userMessage { get; set; }
}