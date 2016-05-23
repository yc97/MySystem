
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;


namespace BusinessLogicLayer
{
    public class MySystemsBLL
    {
        DataAccessLayer.DB_Service currentDB = null;

        public MySystemsBLL() {
            currentDB = new DataAccessLayer.DB_Service();
        }

        public DataTable getSystems(string userName)
        {

            DataTable allSystems = currentDB.getSystems(userName);
            return allSystems;
        }

        public DataTable getAllNodes(int mySystemsID)
        {
            DataTable nodes = currentDB.getAllNodes(mySystemsID);
            return nodes;
        }

        public DataTable getAllDevice()
        {
            DataTable devices = currentDB.getAllDevice();
            return devices;
        }

        public void saveNodeInfo(int nodeID, string device, string signalType, int chNO)
        {
            currentDB.saveNodeInfo(nodeID, device, signalType, chNO);
        }
    }
}
