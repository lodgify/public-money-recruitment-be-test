namespace VacationRental.Infrastructure
{  
    public abstract class RequestCollectionBase
    {
        #region Fields
        private int _pageNumber;

        private int _itemsPerPage;
    
        #endregion

        #region Properties
        
        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                _pageNumber = value;
            }
        }


        public int ItemsPerPage
        {
            get
            {
                return _itemsPerPage;
            }
            set
            {
                _itemsPerPage = value;
            }
        }
        
        #endregion
    }
}
