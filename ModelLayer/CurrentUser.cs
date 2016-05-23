using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class CurrentUser
    {
        private static Users current_user = null;

        public static Users currentUser
        {
            get { return current_user; }
            set { current_user = value; }
        }
    }
}
