using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using VacationRental.Models;
using VacationRental.Models.Rental;

namespace VacationRental.DAL
{
    public class Rentals
    {

        #region SELECTS 

        public static List<RentalViewModel> getRentals()
        {
            List<RentalViewModel> result = new List<RentalViewModel>();
            SqlHelper sh = new SqlHelper();
            string commandText = "Select * from Rentals";
            using (SqlDataReader reader = sh.ExecuteReader(commandText, System.Data.CommandType.Text, null))
            {
                while (reader.Read())
                {
                    RentalViewModel rental = new RentalViewModel();
                    rental.Id = int.Parse(reader["Id"].ToString());
                    rental.PreparationTimeInDays = int.Parse(reader["PreparationTimeInDays"].ToString());
                    rental.Units = int.Parse(reader["Units"].ToString());
                    
                    result.Add(rental);
                }
            }

            return result;
        }

        


        public static RentalViewModel getRentalById(int id)
        {
            RentalViewModel result = new RentalViewModel();
            SqlHelper sh = new SqlHelper();
            string commandText = "Select *  from Rentals where Id = @ID";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ID", id);
            using (SqlDataReader reader = sh.ExecuteReader(commandText, System.Data.CommandType.Text, null))
            {
                while (reader.Read())
                {
                   
                    result.Id = int.Parse(reader["Id"].ToString());
                    result.PreparationTimeInDays = int.Parse(reader["PreparationTimeInDays"].ToString());
                    result.Units = int.Parse(reader["Units"].ToString());

                   
                }
            }

            return result;
        }


        #endregion


        #region UPDATES /INSERTS/ DELETES  
        public static int update(RentalViewModel rental)
        {
            int result = 0;
            SqlHelper sh = new SqlHelper();
            string commandText = "Update Rentals set PreparationTimeInDays = @PREPARATIONTIMEINDAYS, Units= @UNITS where Id = @ID ";
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@ID", rental.Id);
            parameters[1] = new SqlParameter("@PREPARATIONTIMEINDAYS", rental.PreparationTimeInDays);
            parameters[2] = new SqlParameter("@UNITS", rental.Units);
            
            result = sh.ExecuteNonQuery(commandText, System.Data.CommandType.Text, parameters);
            return result;
        }


        public static int insert(RentalViewModel rental)
        {
            int result = 0;
            SqlHelper sh = new SqlHelper();
            string commandText = "Insert into Rentals (PreparationTimeInDays, Units) values (@PREPARATIONTIMEINDAYS, @UNITS )";
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[1] = new SqlParameter("@PREPARATIONTIMEINDAYS", rental.PreparationTimeInDays);
            parameters[2] = new SqlParameter("@UNITS", rental.Units);
            result = sh.ExecuteNonQuery(commandText, System.Data.CommandType.Text, parameters);
            return result;
        }

        public static int delete(int id)
        {
            int result = 0;
            SqlHelper sh = new SqlHelper();
            string commandText = "Delete from Rentals  where Id = @ID";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ID", id);
            result = sh.ExecuteNonQuery(commandText, System.Data.CommandType.Text, parameters);
            return result;
        }

    }

    #endregion
}

