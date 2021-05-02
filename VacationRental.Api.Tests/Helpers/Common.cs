using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace VacationRental.Api.Tests.Helpers
{
    internal static class Common
    {
        public static void OK(HttpResponseMessage m) 
        {
            Assert.True(m.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, m.StatusCode);
        }

        public static void Unprocessable(HttpResponseMessage m)
        {
            Assert.False(m.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, m.StatusCode);
        }        
        
        public static void Compare(HttpResponseMessage m, HttpStatusCode code)
        {
            Assert.Equal(code, m.StatusCode);
        }
    }
}
