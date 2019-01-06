using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationRepository.Entities
{
    public class DynamoTable
    {
        public string PrimaryKey { get; set; }
        public string HashKey { get; set; }
        public string RangeKey { get; set; }
    }
}
