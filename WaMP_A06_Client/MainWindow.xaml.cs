using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/*
 * Programmer   : Colby Taylor
 * Class        : PROG2121 Windows and Mobile Programming
 * Assignment   : A06 Service
 * File         : MainWindow.xaml.cs for use with project WaMP_A06_Service
 * Date         : 11/25/2021
 * Description  : holds the code and decision logic for the client side of the 
 *              : high low game. simple input validation before sending the
 *              : game data to the server for processing. 
 */



namespace WaMP_A06_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        gameInfo game = new gameInfo();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            int guess;
            bool isSuccess = Int32.TryParse(guessText.Text, out guess);
            if (!isSuccess)
            {
                msgText.Text += "Please enter a valid Number";
            }
            else if (msgText.Text == "You Win!! Please press Start Game to play again")
            {
                msgText.Text = "You Win!! Please press Start Game to play again";
            }
            else
            {
                if (guess <= Int32.Parse(game.BtmLim))
                {
                    msgText.Text += "Please enter a Number greater then " + game.BtmLim;
                }
                if (guess > Int32.Parse(game.Range))
                {
                    msgText.Text += "Please enter a Number less then " + game.Range;
                }
                else if(guess >= Int32.Parse(game.BtmLim) && guess <= Int32.Parse(game.Range))
                {
                    string gameState = ConnectClient(ipText.Text, portText.Text, game.Uid + "|" + "guess" + "|" + guessText.Text);
                    string[] splitter = gameState.Split('|');
                    if (splitter[1] == "win")
                    {
                        msgText.Text = "You Win!! Please press Start Game to play again";
                    }
                    else
                    {
                        game.BtmLim = splitter[1];
                        game.Ans = Int32.Parse(splitter[2]);
                        game.Range = splitter[3];
                        msgText.Text = nameText.Text + ", Please enter a guess between " + game.BtmLim + " - " + game.Range;
                        guessText.Text = "";
                    }

                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (msgText.Text.Contains("Are you sure you want to quit? Press Exit Game to Confirm"))
                {
                    ConnectClient(ipText.Text, portText.Text, game.Uid + "|" + "end2");
                    Application.Current.Shutdown();
                }
                string msg = ConnectClient(ipText.Text, portText.Text, game.Uid + "|" + "end");
                string[] splitter = msg.Split('|');
                if (splitter[1] == "Confirm Exit")
                {
                    msgText.Text += "Are you sure you want to quit? Press Exit Game to Confirm";
                }
            }
            catch (Exception ex)
            {
                msgText.Text = "ServerSide Error, Program will close shortly" + ex.ToString();
                Thread.Sleep(500);
                Application.Current.Shutdown();
            }

        }

        static string ConnectClient(String server, String portNum, String message)
        {
            String responseData = String.Empty;
            string msg;
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = Int32.Parse(portNum);
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.


                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                msg = responseData;

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException ex)
            {
                msg = ex.ToString();

            }
            catch (SocketException ex)
            {
                msg = ex.ToString();

            }
            return msg;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string data = ConnectClient(ipText.Text, portText.Text, "0|start");

                string[] splitter = data.Split('|');
                game.Uid = splitter[0];
                game.BtmLim = splitter[1];
                game.Ans = Int32.Parse(splitter[2]);
                game.Range = splitter[3];
                msgText.Text = "Please guess a number between " + game.BtmLim + "-" + game.Range;
            }
            catch (Exception ex)
            {
                msgText.Text = ex.Message;
            }

        }
    }
}
