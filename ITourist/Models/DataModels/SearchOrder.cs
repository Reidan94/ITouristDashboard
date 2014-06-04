using System.Collections.Generic;

namespace ITourist.Models.DataModels
{
    public enum Order
    {
        Default,
        Name,
        Popularity
    }
    public class SearchOrder
    {
        public static Status Default
        {
            get { return new Status((int)Order.Default, new Translation("default", "умолчанию")); }
        }

        public static Status Name
        {
            get { return new Status((int)Order.Name, new Translation("name", "названию")); }
        }

        public static Status Popularity
        {
            get { return new Status((int)Order.Popularity, new Translation("popularity", "популярности")); }
        }

        public static Status GetSearchOrder(int order)
        {
            return order == Default.Id ? Default
                  : order == Name.Id ? Name
                  : order == Popularity.Id ? Popularity
                  : null;
        }
        public static Order GetOrder(int order)
        {
            return  order== (int) Order.Name ? Order.Name
                  : order == (int)Order.Popularity ? Order.Popularity
                  : Order.Default;
        }

        public static IEnumerable<Status> GetAll(Culture culture)
        {
            return new[]
            {
                Default,
                Name,
                Popularity
            };
        }
    }

}