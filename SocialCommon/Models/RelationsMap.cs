using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.Models
{
    public static class RelationsMap
    {
        public static Dictionary<string, Tuple<string, string,string,string>> map { get; set; }

        /// <summary>
        /// tuple:  item1=Model 1
        ///         item2=propertey of model 1
        ///         item3=Model 2  
        ///         item4=propertey f model 2
        /// </summary>

        static RelationsMap()
        {
            map = new Dictionary<string, Tuple<string,string,string, string>>();
            init();
        }

        private static void init()
        {
            map.Add("Publish",new Tuple<string,string,string, string>("User","userID","Post","PostId"));
        }
    }
}
