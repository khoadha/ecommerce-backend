using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Shared
{
    public class MessageDto
    {
        [Required]
        public string From { get; set; }
        public string To { get; set; }
        [Required]
        public string Content { get; set; }

        public DateTime Time { get; set; }
    }

    public class UserList
    {
        public string Email { get; set; }
        public string Username { get; set; }

        public string ImgPath { get; set; }

        public string LastMessage {  get; set; }

        public string CurrentUser { get; set; }
    }
}
