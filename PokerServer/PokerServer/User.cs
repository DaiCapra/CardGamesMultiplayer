using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace PokerServer
{
    public class User
    {
        private string name;
        private string email;
        private string password;
        private string salt;

        public User(string name, string email, string password, string salt)
        {
            this.name = name;
            this.email = email;
            this.password = password;
            this.salt = salt;
        }

        public bool IsPasswordMatch(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] input = Encoding.ASCII.GetBytes(password + salt);
            byte[] hash = md5.ComputeHash(input);

            StringBuilder sB = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sB.Append(hash[i].ToString("X2"));
            }

            if (sB.ToString() == password)
                return true;
            else
                return false;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
