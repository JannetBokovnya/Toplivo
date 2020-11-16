using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Toplivo.Web.Services
{
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public string SearchString { get; set; } //значение строки поиска 
        public string NameSortParm { get; set; } //значение сортировка
        public string Name2SortParm { get; set; } //значение сортировка
        public int TotalPages  // всего страниц
        {
            get
            {
                if (PageSize == 0) return 0;
                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
        }
        public PageInfo()
        {
            PageNumber = 1;
            PageSize = 18;
            TotalItems = 0;
        }
    }
   
}

//public int PageNumber { get; set; } // номер текущей страницы
//public int PageSize { get; set; } // кол-во объектов на странице
//public int TotalItems { get; set; } // всего объектов
//public string SearchString { get; set; } //значение строки поиска 
//public int TotalPages  // всего страниц
//{
//    get
//    {
//        if (PageSize == 0) return 0;
//        return (int)Math.Ceiling((decimal)TotalItems / PageSize);
//    }
//}
//public PageInfo()
//{
//    PageNumber = 1;
//    PageSize = 20;
//    TotalItems = 0;
//}