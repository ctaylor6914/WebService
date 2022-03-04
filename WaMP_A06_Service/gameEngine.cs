using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;


/*
 * Programmer   : Colby Taylor
 * Class        : PROG2121 Windows and Mobile Programming
 * Assignment   : A06 Service
 * File         : gamEngine.cs for use with project WaMP_A06_Service
 * Date         : 11/25/2021
 * Description  : this class drives the game logic on the serverside.
 *              : this class parses the string holding the game data
 *              : and adds the info to the player repo class
 */

namespace WaMP_A06_Service
{
    class gameEngine
    {
        int ans;
        public string playGame(string cmd, playerRepo repo)
        {
            try
            {
                string[] splitter = cmd.Split('|');
                int range;
                Random rand = new Random();

                switch (splitter[1])
                {
                    case "start":
                        Guid g = Guid.NewGuid();
                        splitter[0] = g.ToString();
                        range = Int32.Parse(ConfigurationManager.AppSettings.Get("range"));
                        ans = rand.Next(range);
                        string param = "|" + "0" + "|" + ans + "|" + range;
                        cmd = g.ToString() + param;
                        repo.Set(g.ToString(), param);
                        break;
                    case "end":
                        string msg = "|Confirm Exit";
                        cmd = splitter[0] + msg;
                        break;
                    case "end2":
                        cmd = "SessionEnd";
                        repo.Delete(splitter[0]);
                        break;
                    case "guess":
                        string gameState = repo.Get(splitter[0]);
                        param = Guess(gameState, Int32.Parse(splitter[2]));
                        if (param == "|win")
                        {
                            repo.Delete(splitter[0]);
                        }
                        else
                        {
                            repo.Set(splitter[0], param);
                        }
                        cmd = splitter[0] + param;
                        break;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
            }
            

            return cmd;
        }

        /* function         : Guess()
        * parameters       :string gameState - string value of the stat of the game
        *                  :int guess - int value of the user guess
        * return value     :returns string to continue the game
        * description      :with the parameters entered this fucntion does logic
        *                  : to maintain the game activity and state
        *                  :decides whether it was a in or the game continues
        */
        public static string Guess(string gameState, int guess)
        {
            string param = "";

            try
            {
                
                string[] splitter = gameState.Split('|');
                int btmlim = Int32.Parse(splitter[1]);
                int ans = Int32.Parse(splitter[2]);
                int range = Int32.Parse(splitter[3]);

                if (guess == ans)
                {
                    param = "|win";
                    return param;
                }
                if (guess < ans)
                {
                    btmlim = guess;
                }
                if (guess > ans)
                {
                    range = guess;
                }
                param = "|" + btmlim + "|" + ans + "|" + range;
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
            }
            
            return param;
        }
    }
}
