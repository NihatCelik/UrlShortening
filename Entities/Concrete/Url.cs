using Entities.Abstract;
using Entities.Enums;
using System;

namespace Entities.Concrete
{
    public class Url : IEntity
    {
        public int Id { get; set; }

        public string LongUrl { get; set; }

        public string ShortUrl { get; set; }

        public UrlType UrlType { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
