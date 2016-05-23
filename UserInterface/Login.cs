using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using ModelLayer;

namespace UserInterface
{
    public partial class Login : Form
    {
        BusinessLogicLayer.LoginBLL loginBLL = null;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            loginBLL = new BusinessLogicLayer.LoginBLL();
            chk_RememberMe.Checked = true;
            //设置用户名下拉框
            cmb_UserName.DataSource = loginBLL.getLocalUsers();
            cmb_UserName.DisplayMember = "user";
            cmb_UserName.ValueMember = "pass";
            cmb_UserName.Select();
            //设置密码文本框
            txt_PassWord.Text = cmb_UserName.SelectedValue.ToString();
            
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            string userName = cmb_UserName.Text;
            string passWord = txt_PassWord.Text;
            CurrentUser.currentUser = new Users(userName, passWord);
            
            bool isSucceed = loginBLL.Login();
            if (isSucceed)
            {
                //验证成功
                CurrentUser.currentUser.userID = loginBLL.getUserID(userName);
                loginBLL.LoginDB_Close();
                this.DialogResult = DialogResult.OK;

                if (chk_RememberMe.Checked)
                {
                    //保存用户名密码
                    loginBLL.saveLocalUsers(userName, CurrentUser.currentUser.passWord_MD5);
                }
                else
                {
                    //只存用户名
                    loginBLL.saveLocalUsers(userName);
                }
            }
            else
            {
                loginBLL.LoginDB_Close();
                status.Text = "密码或用户名错误！";
            }


            
        }

        private void cmb_UserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_PassWord.Text = cmb_UserName.SelectedValue.ToString();
        }






    }
}
