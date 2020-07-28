using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsShop
{
    class ComicClass
    {
        //Поля комикса ID, URL изображения, название, описание, цена, количество, автор, жанр, издатель, художник.
        public int ID { get; set; }
        public string PicURL { get; set; }
        public string name_comic { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int count { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public string publish { get; set; }
        public string artist { get; set; }

        //конструктор класса заполняет поля пустыми значениями.
        public ComicClass()
        {
            ID =0;
            PicURL = "";
            name_comic = "";
            description = "";
            price = 0;
            count = 0;
            author = "";
            genre = "";
            publish = "";
            artist = "";
        }
    }
}
