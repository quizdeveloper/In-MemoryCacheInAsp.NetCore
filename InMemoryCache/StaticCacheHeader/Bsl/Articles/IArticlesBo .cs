using StaticCacheHeader.Bsl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticCacheHeader.Bsl.Articles
{
    public interface IArticlesBo
    {
        List<ArticleEntity> GetList();
    }
}
