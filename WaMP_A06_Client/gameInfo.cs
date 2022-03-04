using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Programmer   : Colby Taylor
 * File         : gameInfo.cs for use with project WaMP_A06_Service
 * Date         : 11/25/2021
 * Description  : holds the data for the incoming game state
 */


namespace WaMP_A06_Client
{
    class gameInfo
    {
        private string uid;
        private string btmLim;
        private int ans;
        private string range;

        public string Range { set; get; }

        public int Ans { set; get; }

        public string BtmLim { set; get; }
        public string Uid { set; get; }
    }
}
