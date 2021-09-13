using System.Collections.Generic;

namespace VacationRental.Infrastructure
{
    public abstract class ResponseBase
    {
        #region Properties
        public bool Success { get; set; }

        public string Message { get; set; }

        public IEnumerable<string> ValidationMessages { get; set; }
        #endregion
    }
}
