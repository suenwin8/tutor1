using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutor1.Models.DTO
{
    public class HTTPContextDTO
    {
        //Request
        public HTTPContextDTO(string method, string path, QueryString queryString, string headers, string scheme, HostString host, string body)
        {//Console.WriteLine($"HTTP request information:\n" +
         //    $"\tMethod: {httpContext.Request.Method}\n" +
         //    $"\tPath: {httpContext.Request.Path}\n" +
         //    $"\tQueryString: {httpContext.Request.QueryString}\n" +
         //    $"\tHeaders: {FormatHeaders(httpContext.Request.Headers)}\n" +
         //    $"\tSchema: {httpContext.Request.Scheme}\n" +
         //    $"\tHost: {httpContext.Request.Host}\n" +
         //    $"\tBody: {await ReadBodyFromRequest(httpContext.Request)}");
            Method = method;
            Path = path;
            QueryString = queryString;
            Headers = headers;
            Scheme = scheme;
            Host = host;
            Body = body;           
        }

        public HTTPContextDTO(int statusCode, string contentType,string headers, string body)
        {
            StatusCode = statusCode;
            ContentType = contentType;
            Headers = headers;
            Body = body;
        }

        public string Method;
        public string Path;
        public QueryString QueryString;
        public string Headers;
        public string Scheme;
        public HostString Host;
        public string Body;
        public int StatusCode;
        public string ContentType;

        

     
    }
}
