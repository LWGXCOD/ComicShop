using ComicsShop.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsShop
{
    //статический класс содержащий информацию о авторизированном пользователе, в том исле содержимое его корзины и его заказы.
    static class UserData
    {
        public static int ID { get; set; }
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static string Email { get; set; }
        public static bool Access { get; set; }
        public static string Name { get; set; }
        public static string SurName { get; set; }
        public static string Mobile { get; set; }
        //заказы пользователя
        public static Dictionary<int, OrderClass> UserOrders =  new Dictionary<int, OrderClass> { };
        //корзина пользователя
        public static OrderClass Bascket = new OrderClass();

    }
}
