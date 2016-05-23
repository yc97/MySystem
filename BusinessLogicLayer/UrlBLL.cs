using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BusinessLogicLayer
{
    public class UrlBLL
    {
        DataAccessLayer.DB_Service currentDB = null;

        public UrlBLL()
        {
            currentDB = new DataAccessLayer.DB_Service();
        }

        public DataTable getUrlHistory() 
        {
            int userID = ModelLayer.CurrentUser.currentUser.userID;
            DataTable dt = null;
            dt = currentDB.getUrlHistory(userID);
            return dt;
        }

        public void addUrlHistory(string url)
        {
            int userID = ModelLayer.CurrentUser.currentUser.userID;
            currentDB.addUrlHistory(userID, url);
        }

        public void clearUrlHistory()
        {
            int userID = ModelLayer.CurrentUser.currentUser.userID;
            currentDB.clearUrlHistory(userID);
        }
    }
}
