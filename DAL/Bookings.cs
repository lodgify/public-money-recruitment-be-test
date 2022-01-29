using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using VacationRental.Models.Booking;

namespace VacationRental.DAL
{
   public class Bookings
    {

        #region SELECTS 

        public static List<BookingViewModel> getBookings()
        {
            List<BookingViewModel> result = new List<BookingViewModel>();
            SqlHelper sh = new SqlHelper();
            string commandText = "Select *  from Bookings";
            using (SqlDataReader reader = sh.ExecuteReader(commandText, System.Data.CommandType.Text, null))
            {
                while (reader.Read())
                {
                    BookingViewModel booking = new BookingViewModel();
                    booking.Id = int.Parse(reader["Id"].ToString());
                    booking.Nights = int.Parse(reader["Nights"].ToString());
                    booking.RentalId = int.Parse(reader["RentalId"].ToString());
                    booking.Start = DateTime.Parse(reader["Start"].ToString());
                    booking.Unit = int.Parse(reader["Unit"].ToString());

                    result.Add(booking);
                }
            }

            return result;
        }


        public static List<BookingViewModel> getBookingsByRentalId(int rentalId)
        {
            List<BookingViewModel> result = new List<BookingViewModel>();
            SqlHelper sh = new SqlHelper();
            string commandText = "Select *  from Bookings where RentalId = @RENTAL_ID";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@RENTAL_ID", rentalId);
            using (SqlDataReader reader = sh.ExecuteReader(commandText, System.Data.CommandType.Text, null))
            {
                while (reader.Read())
                {
                    BookingViewModel booking = new BookingViewModel();
                    booking.Id = int.Parse(reader["Id"].ToString());
                    booking.Nights = int.Parse(reader["Nights"].ToString());
                    booking.RentalId = int.Parse(reader["RentalId"].ToString());
                    booking.Start = DateTime.Parse(reader["Start"].ToString());
                    booking.Unit = int.Parse(reader["Unit"].ToString());

                    result.Add(booking);
                }
            }

            return result;
        }


        public static BookingViewModel getBookingsById(int id)
        {
            BookingViewModel result = new BookingViewModel();
            SqlHelper sh = new SqlHelper();
            string commandText = "Select *  from Bookings where Id = @ID";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ID", id);            
            using (SqlDataReader reader = sh.ExecuteReader(commandText, System.Data.CommandType.Text, null))
            {
                while (reader.Read())
                {

                    result.Id = int.Parse(reader["Id"].ToString());
                    result.Nights = int.Parse(reader["Nights"].ToString());
                    result.RentalId = int.Parse(reader["RentalId"].ToString());
                    result.Start = DateTime.Parse(reader["Start"].ToString());
                    result.Unit = int.Parse(reader["Unit"].ToString());

                    
                }
            }

            return result;
        }


        #endregion


        #region UPDATES /INSERTS/ DELETES  
        public static int update(BookingViewModel booking)
        {
            int result = 0;
            SqlHelper sh = new SqlHelper();
            string commandText = "Update Bookings set Nights = @NIGHTS, RentalId= @RENTAL_ID, Start = @START, Unit = @UNIT  where Id = @ID ";
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@ID", booking.Id);
            parameters[1] = new SqlParameter("@NIGHTS", booking.Nights);
            parameters[2] = new SqlParameter("@RENTAL_ID", booking.RentalId);
            parameters[3] = new SqlParameter("@START", booking.Start);
            parameters[4] = new SqlParameter("@UNIT", booking.Unit);
            result = sh.ExecuteNonQuery(commandText, System.Data.CommandType.Text, parameters);
            return result;
        }


        public static int insert(BookingBindingModel booking)
        {
            int result = 0;
            SqlHelper sh = new SqlHelper();
            string commandText = "Insert into Bookings (Nights, RentalId, Start, Unit) values (@NIGHTS, @RENTAL_ID, @START, @UNIT )";
            SqlParameter[] parameters = new SqlParameter[4];           
            parameters[0] = new SqlParameter("@NIGHTS", booking.Nights);
            parameters[1] = new SqlParameter("@RENTAL_ID", booking.RentalId);
            parameters[2] = new SqlParameter("@START", booking.Start);
            parameters[3] = new SqlParameter("@UNIT", booking.Unit);
            result = sh.ExecuteNonQuery(commandText, System.Data.CommandType.Text, parameters);
            return result;
        }

        public static int delete(int id)
        {
            int result = 0;
            SqlHelper sh = new SqlHelper();
            string commandText = "Delete from Bookings  where Id = @ID";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ID", id);
            result = sh.ExecuteNonQuery(commandText, System.Data.CommandType.Text, parameters);
            return result;
        }

    }

    #endregion

}


