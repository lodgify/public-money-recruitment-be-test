namespace VacationRental.Api.Tests.ApiRoutes
{
    public static class CalendarApiRoute
    {
        private const string _get = "/api/v1/calendar?rentalId={0}&start={1}&nights={2}";
        
        public static string Get(int rentalId, string start, int nights)
        {
            return string.Format(_get, rentalId, start, nights);
        }
    }
}
