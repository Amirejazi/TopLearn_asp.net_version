using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.DTOs.Order;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Order;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services
{
    public class OrderService : IOrderService
    {
        private TopLearnContext _context;
        private IUserService _userService;

        public OrderService(TopLearnContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public int AddOrder(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            Order order = _context.Orders.FirstOrDefault(o => o.User.UserId == userId && !o.IsFinaly);

            var Course = _context.Courses.Find(courseId);

            if (order == null)
            {
                order = new Order()
                {
                    UserId = userId,
                    IsFinaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = Course.CoursePrice,
                    OrderDetails = new List<OrderDetail>()
                    { 
                        new OrderDetail()
                        {
                            CourseId = courseId,
                            Count = 1,
                            Price = Course.CoursePrice
                        }
                    }
                };
                _context.Orders.Add(order);
            }
            else
            {
                var orderDetail =
                    _context.OrderDetails.FirstOrDefault(o => o.OrderId == order.OrderId && o.CourseId == courseId);
                if (orderDetail != null)
                {
                    orderDetail.Count += 1;
                    _context.OrderDetails.Update(orderDetail);
                    _context.SaveChanges();
                }
                else
                {
                    orderDetail = new OrderDetail()
                    {
                        Count = 1,
                        CourseId = courseId,
                        OrderId = order.OrderId,
                        Price = Course.CoursePrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                    _context.SaveChanges();
                }
                UpdatePriceOrder(order.OrderId);
            }

            _context.SaveChanges();
            return order.OrderId;

        }

        public void UpdatePriceOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            order.OrderSum = _context.OrderDetails.Where(o => o.OrderId == order.OrderId).Sum(o => o.Price * o.Count);
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public Order GetOrderForUserPanel(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od=> od.Course)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
        }

        public Order GetOrdeById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public bool FinlayOrder(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            var order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);

            if (order == null || order.IsFinaly)
                return false;

            if (_userService.BalanceWallet(userName) >= order.OrderSum)
            {
                order.IsFinaly = true;
                _userService.AddWallet(new Wallet()
                {
                    IsPay = true,
                    CreateDate = DateTime.Now,
                    UserId = userId,
                    Description = "فاکتور شما #"+order.OrderId.ToString(),
                    Amount = order.OrderSum,
                    TypeId = 2
                });
                _context.Orders.Update(order);
                foreach (var detail in order.OrderDetails)
                {
                    _context.UserCourses.Add(new UserCourse()
                    {
                        CourseId = detail.CourseId,
                        UserId = userId
                    });
                }
                _context.SaveChanges();
                return true;
            }

            return false;

        }

        public List<Order> getUserOrders(string userName)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public bool IsUserInCourse(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            return _context.UserCourses.Any(uc => uc.UserId==userId && uc.CourseId==courseId);
        }

        public DiscountUseType UseDiscount(int orderId, string code)
        {
            var discount = _context.Discounts.SingleOrDefault(d => d.DiscountCode == code);

            if (discount == null)
            {
                return DiscountUseType.NotFound;
            }

            if (discount.StartDate != null && discount.StartDate < DateTime.Now)
            {
                return DiscountUseType.ExpiredDate;
            }

            if (discount.EndDate != null && discount.EndDate > DateTime.Now)
            {
                return DiscountUseType.ExpiredDate;
            }

            if (discount.UsableCount != null && discount.UsableCount < 1)
            {
                return DiscountUseType.Finished;
            }

            var order = GetOrdeById(orderId);

            if (_context.UserDiscountCodes.Any(c => c.UserId == order.UserId && c.DiscountId == discount.DiscountId))
            {
                return DiscountUseType.UserUsed;
            }

            order.OrderSum = order.OrderSum - (order.OrderSum * discount.DiscountPercent / 100);
            UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }

            _context.Discounts.Update(discount);
            _context.UserDiscountCodes.Add(new UserDiscountCode()
            {
                UserId = order.UserId,
                DiscountId = discount.DiscountId,
            });
            _context.SaveChanges();

            return DiscountUseType.Success;
        }

        public void AddDiscount(Discount discount)
        {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
        }

        public List<Discount> GetAllDiscount()
        {
            return _context.Discounts.ToList();
        }

        public Discount GetDiscountById(int discountId)
        {
            return _context.Discounts.Find(discountId);
        }

        public void UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            _context.SaveChanges();
        }

        public bool IsExistCode(string code)
        {
            return _context.Discounts.Any(d => d.DiscountCode == code);
        }
    }
}
