using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? SubmitTime { get; set; }
    }
}
