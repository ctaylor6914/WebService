using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;

/*
 * Programmer   : Colby Taylor
 * Class        : PROG2121 Windows and Mobile Programming
 * Assignment   : A06 Service
 * File         : gameInfo.cs for use with project WaMP_A06_Service
 * Date         : 11/25/2021
 * Description  : This class starts the TCP listener which waits for a 
 *              : client 
 */

namespace WaMP_A06_Service
{
    class Listener
    {
        int port = 0;
        IPAddress ipAddress = null;
        TcpListener server = null;
        int BUFFERSIZE = 1024;
        public volatile bool Run = true;

        public Listener()
        {
            
            try
            {
                port = Int32.Parse(ConfigurationManager.AppSettings.Get("port"));
                ipAddress = IPAddress.Parse(ConfigurationManager.AppSettings.Get("ipAddress"));
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
            }
            
        }

        //public Listener(string ipAddress, int port)
        //{
        //    // for testing
        //    // TODO: needs validation
        //    this.port = port;
        //    this.ipAddress = IPAddress.Parse(ipAddress);
        //}


        public void Listen()
        {
            try
            {
                server = new TcpListener(ipAddress, port);
                server.Start();
                byte[] bytes = new byte[BUFFERSIZE];
                string request = null;
                string response = null;
                gameEngine game = new gameEngine();
                playerRepo repo = new playerRepo();

                while (Run)
                {
                    if (server.Pending())
                    {
                        TcpClient client = server.AcceptTcpClient();
                        NetworkStream stream = client.GetStream();
                        int i = stream.Read(bytes, 0, bytes.Length);
                        request = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        response = game.playGame(request, repo);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                        stream.Write(msg, 0, msg.Length);
                        client.Close();
                        //ParameterizedThreadStart ts = new ParameterizedThreadStart(game.Worker);
                        //Thread clientThread = new Thread(ts);
                        //clientThread.Start(client);
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
            }
            catch(SocketException ex)
            {
                Logger.Log(ex.Message);
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                if(server != null)
                {
                    server.Stop();
                }
            }
        }
    }
}
