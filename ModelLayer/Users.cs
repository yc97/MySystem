///////////////////////////////////////////////////////////
//  Users.cs
//  Implementation of the Class Users
//  Generated by Enterprise Architect
//  Created on:      21-����-2016 11:39:37
//  Original author: Administrator
///////////////////////////////////////////////////////////

namespace ModelLayer
{
	public class Users {

        public int userID;
        public string userName;
		private string passWord;
        public string passWord_MD5;
		public PermissionTypes permissions = PermissionTypes.None;


		public Users(string _userName, string _password){
            userName = _userName;
            passWord = _password;
            passWord_MD5 = DESEncrypt.Encrypt(_password);
            System.Console.WriteLine("passWord_MD5:");
            System.Console.WriteLine(passWord_MD5);
            permissions = PermissionTypes.Default;
		}

		~Users(){

		}

		public virtual void Dispose(){

		}

	}//end Users

}//end namespace MySystem//end namespace MySystem