using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindAndBook.API.App_Start
{
    public static class Constants
    {
        public static string REQUIRED_RESTAURANT_ID = "Restaurant Id is required.";
        public static string REQUIRED_USER_ID = "User Id is required.";
        public static string REQUIRED_BOOKING_ID = "Booking Id is required.";
        public static string NO_AVAILABLE_TABLES = "All tables are reserved.";
        public static string FORBIDDEN_CANCEL_BOOKING = "You can cancel only your bookings.";
        public static string INCORRECT_AVERAGE_BILL = "Average Bill must be a valid number.";
        public static string FORBIDDEN_MANAGER_RESTAURANTS_LISTING = "You can see only list of your restaurants.";
        public static string FORBIDDEN_EDIT_RESTAURANT = "Only manager of this restaurant can edit it.";
        public static string FORBIDDEN_DELETE_RESTAURANT = "Only manager of this restaurant can delete it.";
        public static string FORBIDDEN_GET_PROFILE = "You can see only your profile.";
        public static string FORBIDDEN_DELETE_PROFILE = "You can delete only your profile.";
        public static string USERNAME_CONFLICT = "There is already user with that username.";
        public static string MANAGER_ROLE = "Manager";
    }
}