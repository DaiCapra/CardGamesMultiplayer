using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace PokerClient
{
    public partial class FormLogin : Form
    {
        bool CONNECT_TO_SERVER = true; //remove later => form_activated

        string ip = "127.0.0.1";
        int port = 9010;

        bool firstTime = true;

        Client client;
        FormLobby form2;

        public FormLogin()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }
        private void GoToLobby()
        {
            form2 = new FormLobby(client);
            form2.Show();

            this.Opacity = 0.0f;
            this.ShowInTaskbar = false;
            this.Hide();
                  
        }

        private void btn_signin_Click(object sender, EventArgs e)
        {
            //GoToLobby();
            if (client.Listener.Connected)
            {
                client.SendAuthenticate(tb_username.Text, tb_password.Text);
               
                Thread t2 = new Thread(client.Listener.Read);
                t2.Start();
            }
        }

        private void frm_login_Activated(object sender, EventArgs e)
        {
            if (firstTime)
            {
                client = new Client(ip, port, lb_statusBox);
                if (CONNECT_TO_SERVER)
                {

                    Thread t1 = new Thread(client.ConnectToServer);
                    t1.Start();
                }
                firstTime = false;
            }
        }
    }
}
