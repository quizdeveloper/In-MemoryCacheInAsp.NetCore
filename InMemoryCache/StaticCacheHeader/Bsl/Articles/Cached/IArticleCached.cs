using StaticCacheHeader.Bsl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticCacheHeader.Bsl.Articles.Cached
{
    public interface IArticleCached
    {
        List<ArticleEntity> GetList();
    }
}
