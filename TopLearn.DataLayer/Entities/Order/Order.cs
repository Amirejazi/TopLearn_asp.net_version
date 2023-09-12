using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Order
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        
        [Required]
        public int UserId { get; set; }

        [Required]
        public int OrderSum { get; set; }

        public bool IsFinaly { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        #region Relations

        public User.User User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        #endregion


    }
}
