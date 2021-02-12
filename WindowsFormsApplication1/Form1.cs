using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool connected = false;
        Socket clientSocket;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;

            int portNum;
            if (Int32.TryParse(textBox_port.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);
                    connected = true;
                    logs.AppendText("Connection established...\n");

                    string email = textBox_email.Text;
                    string name = textBox_name.Text;
                    string message = email + " " + name;
                    string token = "";

                    if (message != "" && message.Length <= 64)
                    {
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(message);
                        clientSocket.Send(buffer);
                        logs.AppendText("Message sent: " + message + "\n");
                    }

                    if (connected)
                    {

                        Byte[] buffer = new Byte[64];
                        clientSocket.Receive(buffer);

                        string incomingMessage = Encoding.Default.GetString(buffer);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                        logs.AppendText("Server: " + incomingMessage + "\n");
                        token = incomingMessage;
                    }

                    string retrieved_chars = "";
                    foreach (char index in token) 
                    {
                        retrieved_chars += email[index - '0'];
                    }

                    string message2 = retrieved_chars + " " + name;

                    if (message2 != "" && message2.Length <= 64)
                    {
                        Byte[] buffer = new Byte[64];
                        buffer = Encoding.Default.GetBytes(message2);
                        clientSocket.Send(buffer);
                        logs.AppendText("Message sent: " + message2 + "\n");
                    }

                    if (connected)
                    {

                        Byte[] buffer = new Byte[64];
                        clientSocket.Receive(buffer);

                        string incomingMessage = Encoding.Default.GetString(buffer);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                        logs.AppendText("Server: " + incomingMessage + "\n");
                    }

                    clientSocket.Close();
                    connected = false;
                }
                catch
                {
                    logs.AppendText("Problem occured while connecting...\n");
                }
            }
            else
            {
                logs.AppendText("Problem occured while connecting...\n");
            }

        }
    }
}
