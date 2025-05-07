using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PropertyManager.Domain.Common;

    public class TResponse
    {
        public string? Message { get; set; }
        public string? Result { get; set; } = string.Empty;
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public dynamic? Data { get; set; }

        public IEnumerable<Error> Errors { get; set; } = [];
    }

