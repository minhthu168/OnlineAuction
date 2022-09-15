using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using eProject.Models;
namespace eProject.Repository
{
    public interface INews
    {
        public List<News> GetNews();
        public News OneNews(int id);
        public void CreateNews(News newNews);
        public void UpdateNews(News editNews);
        public void RemoveNews(int id);
    }
}
