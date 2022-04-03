namespace VacationRental.Api.Tests.ApiRoutes
{
    public static class RentalApiRoute
    {
        private const string _post = "/api/v1/rentals";
        private const string _put = "/api/v1/rentals/{0}";
        private const string _get = "/api/v1/rentals/{0}";

        public static string Post()
        {
            return _post;
        }
        
        public static string Put(int rentalId)
        {
            return string.Format(_put, rentalId);
        }
        
        public static string Get(int rentalId)
        {
            return string.Format(_get, rentalId);
        }
    }
}
