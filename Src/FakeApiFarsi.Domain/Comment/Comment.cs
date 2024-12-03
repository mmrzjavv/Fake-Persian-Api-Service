using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeApiFarsi.Domain.Comment
{
    public class Comment
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

}
