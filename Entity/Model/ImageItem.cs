using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class ImageItem
    {
        public int Id { get; set; }
        public string UrlImage { get; set; }
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime DeleteAt { get; set; }
        public ICollection<Item> Item { get; set; }
    }
}
