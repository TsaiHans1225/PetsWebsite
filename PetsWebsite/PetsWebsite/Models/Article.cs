using System;
using System.Collections.Generic;

namespace PetsWebsite.Models
{
    public partial class Article
    {
        public int ArticleId { get; set; }
        public int? RestaurantId { get; set; }
        public string Title { get; set; } = null!;
        public string Contents { get; set; } = null!;

        public virtual Restaurant? Restaurant { get; set; }
    }
}
