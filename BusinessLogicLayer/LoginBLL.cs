using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Data;
using ModelLayer;

namespace BusinessLogicLayer
{
    public class LoginBLL
    {
        DataAccessLayer.DB_Service currentDB = null;

        public LoginBLL() {
            currentDB = new DataAccessLayer.DB_Service();
        }

        public bool Login() 
        { 
            string userName = CurrentUser.currentUser.userName;
            string passWord_MD5 = CurrentUser.currentUser.passWord_MD5;
            bool isSuceeed = currentDB.Login(userName, passWord_MD5);
            return isSuceeed;
        }

        public int getUserID(string userName) 
        {
            int userID;
            DataTable userID_DT = currentDB.getUserID(userName);
            userID = System.Convert.ToInt32(userID_DT.Rows[0]["usersID"]);
            return userID;
        }

        public void saveLocalUsers(string userName)
        {
            saveLocalUsers(userName, "");
        }

        public void saveLocalUsers(string userName, string passWord_MD5)
        {
            if (!Properties.Settings.Default.u.Contains(userName))
            {
                Properties.Settings.Default.u.Add(userName);
                Properties.Settings.Default.p.Add(passWord_MD5);
            }
            else
            {
                int i = Properties.Settings.Default.u.IndexOf(userName);
                Properties.Settings.Default.p[i] = passWord_MD5;
            }
            Properties.Settings.Default.lastu = userName;
            Properties.Settings.Default.Save();
        }

        public DataTable getLocalUsers() 
        {
            DataTable userDT = new DataTable();
            userDT.Columns.Add("user");
            userDT.Columns.Add("pass");
            
            DataRow dr = userDT.NewRow();
            string lastUser = Properties.Settings.Default.lastu;
            dr["user"] = lastUser;
            int lastId = Properties.Settings.Default.u.IndexOf(lastUser);
            dr["pass"] = ModelLayer.DESEncrypt.Decrypt(Properties.Settings.Default.p[lastId]);
            userDT.Rows.Add(dr);

            for (int i = 0; i < Properties.Settings.Default.u.Count; i++) 
            {
                if (i != lastId)
                {
                    dr = userDT.NewRow();
                    dr["user"] = Properties.Settings.Default.u[i];
                    dr["pass"] = ModelLayer.DESEncrypt.Decrypt(Properties.Settings.Default.p[i]);
                    userDT.Rows.Add(dr);
                }
            }
            return userDT;
        }

        public void LoginDB_Close()
        {
            currentDB.Close();
        }
    }
}
