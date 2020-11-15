using StaticCacheHeader.Bsl.Caching;
using StaticCacheHeader.Bsl.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticCacheHeader.Bsl.Articles.Cached
{
    public class ArticleCached : IArticleCached
    {
        private IArticlesBo articlesBo;
        private ICacheBase cacheBase;
        public ArticleCached(IArticlesBo articlesBo, ICacheBase cacheBase)
        {
            this.articlesBo = articlesBo;
            this.cacheBase = cacheBase;
        }

        public List<ArticleEntity> GetList()
        {
            try
            {
                var key = KeyCahe.GenCacheKey("GetListArticles");
                var listArticle = cacheBase.Get<List<ArticleEntity>>(key);
                if (listArticle == null)
                {
                    listArticle = articlesBo.GetList();
                    cacheBase.Add<List<ArticleEntity>>(listArticle, key);
                }

                return listArticle;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
