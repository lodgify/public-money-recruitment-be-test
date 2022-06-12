using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace VacationRental.Domain.Models
{
    public class Error
    {
        [JsonProperty("error")]
        public string message { get; set; }

    }
}
