using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BusinessLogicLayer
{
    public class DataBLL
    {
        DataAccessLayer.DB_Service currentDB = null;
        public DataBLL()
        {
            currentDB = new DataAccessLayer.DB_Service();
        }

        public DataTable getData(string nodeID)
        {

            //DataTable allData = currentDB.getData(nodeID);
            return null;
        }

        public ModelLayer.Nodes getNodeInfo(int nodeID)
        {
            DataTable nodeDT = currentDB.getNodeInfo(nodeID);
            ModelLayer.Nodes nodeInfo = new ModelLayer.Nodes();
            if (nodeDT.Rows.Count > 0)
            {
                nodeInfo.signalType = nodeDT.Rows[0]["signalType"].ToString();
                nodeInfo.device = nodeDT.Rows[0]["device"].ToString();
                nodeInfo.chNO = System.Convert.ToInt32(nodeDT.Rows[0]["chNO"].ToString());
            }
            return nodeInfo;
        }

        public DataTable getMonitorData(int nodeID)
        {
            DataTable dt = currentDB.getMonitorData(nodeID);
            return dt;
        }
    }
}
