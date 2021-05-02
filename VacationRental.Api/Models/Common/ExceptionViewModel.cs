using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Api.Models.Common
{
    public class ExceptionViewModel
    {
        #region constructor
        public ExceptionViewModel()
        {

        }

        public ExceptionViewModel(string message)
        {
            Message = message;
        }

        public ExceptionViewModel(string message, string moreInfo) : this (message)
        {
            MoreInfo = moreInfo;
        }
        #endregion

        public string Message { get; set; }
        public string MoreInfo { get; set; }

        public string ConvertToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
