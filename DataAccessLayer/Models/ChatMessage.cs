using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ChatMessage : BaseEntity
    {
        public string MessageText { get; set; }
        public string ConnectionId { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }

        public DateTime SentDateTime { get; set; }

        public string NewProperty { get; set; }
    }
}
