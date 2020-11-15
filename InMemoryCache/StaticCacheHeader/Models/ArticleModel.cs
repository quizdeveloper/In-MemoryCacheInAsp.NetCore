using StaticCacheHeader.Bsl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticCacheHeader.Models
{
    public class ArticleModel
    {
        public ArticleModel()
        {

        }

        public ArticleModel(ArticleEntity map)
        {
            this.Id = map.Id;
            this.Title = map.Title;
            this.ImageUrl = map.ImageUrl;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

    }
}
