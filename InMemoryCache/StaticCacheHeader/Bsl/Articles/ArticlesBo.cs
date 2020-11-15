using StaticCacheHeader.Bsl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticCacheHeader.Bsl.Articles
{
    public class ArticlesBo : IArticlesBo
    {
        public List<ArticleEntity> GetList()
        {
            var result = new List<ArticleEntity>();
            for (var i = 1; i <= 50; i++)
            {
                result.Add(new ArticleEntity() { 
                  Id = i,
                  Title =" Image title " + i,
                  ImageUrl = string.Format("../images/{0}.jpg", i)
                });
            }

            return result;
        }
    }
}
