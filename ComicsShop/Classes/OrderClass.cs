using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsShop.Classes
{
    class OrderClass
    {
        //Поля заказа: сумма строковое представление строки для записи в таблицу БД,количество различных товаров, Словарь содержащий пару <ID товара; количество товара>.
        public int sum = 0;
        public string OrderString;
        public int CountProducts = 0;
        public Dictionary<int,int> OrderIdCount = new Dictionary<int, int> { };
        //Функция создающая по данным словаря строку для записи ее в таблицу orders  в БД.
        public string GetOrder()
        {
            int count = CountProducts;
            Dictionary<int, int> M = OrderIdCount;
            string ord = "";
            ord += count.ToString() + " ";
            foreach (KeyValuePair<int,int> kvp in OrderIdCount)
            {
                ord += kvp.Key.ToString() + "," + kvp.Value.ToString() + " ";
            }
            OrderString = ord;
            return ord;
        }
        //Функция расшифровывающая строковое представление заказа и возвращающая словарь содержащий пару <ID товара; количество товара>.
        public Dictionary<int, int> GetIds(string ord)
        {
            Dictionary<int, int> M = new Dictionary<int, int> { };
            int count = 0;
            int i = 0;
            while ((ord[i] != ' ') && (i < ord.Length))
            {
                count *= 10;
                count += (int)Char.GetNumericValue(ord[i]);
                i++;
            }
            ord = ord.Remove(0, i + 1);
            for (int j = 0; j < count; j++)
            {
                int id = 0;
                int c = 0;
                i = 0;
                while ((ord[i] != ',') && (i < ord.Length))
                {
                    id *= 10;
                    id += (int)Char.GetNumericValue(ord[i]);
                    i++;
                }
                ord = ord.Remove(0, i + 1);
                i = 0;
                while ((ord[i] != ' ') && (i < ord.Length))
                {
                    c *= 10;
                    c += (int)Char.GetNumericValue(ord[i]);
                    i++;
                }
                ord = ord.Remove(0, i + 1);
                M.Add(id, c);
            }
            return M;
        }

    }
}
