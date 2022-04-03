namespace VacationRental.Api.Tests.ApiRoutes
{
    public static class BookingApiRoute
    {
        private const string _post = "/api/v1/bookings";
        private const string _get = "/api/v1/bookings/{0}";
        
        public static string Post()
        {
            return _post;
        }
        
        public static string Get(int bookingId)
        {
            return string.Format(_get, bookingId);
        }
    }
}
