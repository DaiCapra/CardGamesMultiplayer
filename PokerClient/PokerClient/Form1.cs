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
    public partial class frm_login : Form
    {
        bool CONNECT_TO_SERVER = true; //remove later => form_activated

        string ip = "127.0.0.1";
        int port = 9010;

        Client client; 

        public frm_login()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void btn_signin_Click(object sender, EventArgs e)
        {
            if (client.Listener.Connected)
            {

            }
        }

        private void frm_login_Activated(object sender, EventArgs e)
        {
            if (CONNECT_TO_SERVER)
            {
                client = new Client(ip, port, lb_statusBox);
                Thread t1 = new Thread(client.ConnectToServer);
                t1.Start();
            }
        }
    }
}
