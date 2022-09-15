using eProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eProject.Service
{
    public class NewsServices : Repository.INews
    {
        private readonly Data.DatabaseContext context;
        public NewsServices(Data.DatabaseContext context)
        {
            this.context = context;
        }

        public void CreateNews(News newNews)
        {
            News news = context.News.SingleOrDefault(x => x.NewsId.Equals(newNews.NewsId));
            if (news == null)
            {
                context.News.Add(newNews);
                context.SaveChanges();
            }
            else
            {
                //do nothing
            }
        }

        public List<News> GetNews()
        {
            var res = context.News.ToList();
            if (res != null)
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public News OneNews(int id)
        {
            News news = context.News.SingleOrDefault(x => x.NewsId.Equals(id));
            if (news != null)
            {
                return news;
            }
            else
            {
                return null;
            }
        }

        public void RemoveNews(int id)
        {
            News news = context.News.SingleOrDefault(x => x.NewsId.Equals(id));
            if (news != null)
            {
                context.News.Remove(news);
                context.SaveChanges();
            }
            else
            {
                //do nothing
            }
        }

        public void UpdateNews(News editNews)
        {
            News news = context.News.SingleOrDefault(x => x.NewsId.Equals(editNews.NewsId));
            if (news != null)
            {
                news.Title = editNews.Title;
                news.Content = editNews.Content;
                news.Image = editNews.Image;
                news.CreateDate = DateTime.Now;

                context.SaveChanges();
            }
            else
            {
                //do nothing
            }
        }
    }
}
