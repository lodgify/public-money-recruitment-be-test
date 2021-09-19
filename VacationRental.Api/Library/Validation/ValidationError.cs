using System.Collections.Generic;

namespace VacationRental.Api.Validation
{
    public class Errors
    {
        public List<ErrorsModel> ErrorsModels { get; set; } = new ();
    }

    public class ErrorsModel
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
