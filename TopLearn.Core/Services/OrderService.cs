using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Order;
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
            order.OrderSum = _context.OrderDetails.Where(o => o.OrderId == order.OrderId).Sum(o => o.Price);
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public Order GetOrderForUserPanel(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od=> od.Course)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
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
    }
}
