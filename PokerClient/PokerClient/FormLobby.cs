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

namespace PokerClient
{
    public partial class FormLobby : Form
    {

        FormLogin formLogin;

        public FormLobby(Client client)
        {
            InitializeComponent();
        }

        private void FormLobby_Load(object sender, EventArgs e)
        {
            
        }
    }
}
