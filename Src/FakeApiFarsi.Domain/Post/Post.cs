using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FakeApiFarsi.Domain.Post
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public List<string> Tags { get; set; } = [];
        public List<Comment.Comment> Comments { get; set; } = new List<Comment.Comment>();
    }

}
