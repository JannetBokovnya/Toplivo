//Класс для представление данных из класса T, разбитыми на порции - страницы
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web;

namespace Toplivo.Web.Services
{
    public class PagedCollections<T>
    {
        public IEnumerable<T> PagedItems { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}