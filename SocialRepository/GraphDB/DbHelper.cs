using Neo4j.Driver.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialRepository.GraphDB
{
    static public class DbHelper
    {
        private static readonly IDriver _driver;
        static string uri = "bolt://ec2-34-243-124-142.eu-west-1.compute.amazonaws.com:7687";


         static DbHelper()
        {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic("neo4j", "skay1414"));

        }

        public  static ISession getSession() {
           return _driver.Session();
        }

        /// <summary>
        /// Serialize object to Json without -"-
        /// usefull when passing to object to cypher query for neo4j
        /// </summary>
        public static string ObjectToJson(object obj)
        {
            var serializer = new JsonSerializer();
            var stringWriter = new StringWriter();
            using (var writer = new JsonTextWriter(stringWriter))
            {
                writer.QuoteName = false;
                serializer.Serialize(writer, obj);
            }
            return stringWriter.ToString();
        }

    }

    //public class User
    //{
    //    public string Name { get; set; }
    //    public string lastName { get; set; }
    //    public User()
    //    {

    //    }
    //    public User(string name,string lastName)
    //    {
    //        this.Name = name;
    //        this.lastName = lastName;
    //    }
    //}

}
