using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime PublicDate { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
