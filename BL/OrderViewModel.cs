using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class OrderViewModel
    {

        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string ClientPhone { get; set; }
        public string Email { get; set; }
        public string ClientName { get; set; }
        public string City { get; set; }

        public int NovaPoshta { get; set; }

        public List<Guid> CartItemIds { get; set; } = new List<Guid>();

        public bool IsEmpty
        {
            get => true;
        }
        public OrderViewModel()
        {

        }

        public OrderViewModel(Order order)
        {
            Id = order.Id;
            Status = order.Status;
            OrderDate = order.OrderDate;
            ClientPhone = order.ClientPhone;
            Email = order.Email;
            ClientName = order.ClientName;
            City =  order.City;
            NovaPoshta = order.NovaPoshta;

            CartItemIds = order.CartItems.Select(c => c.Id).ToList();
        }

        public static implicit operator Order(OrderViewModel model)
        {
            return new Order
            {
                Id = model.Id,
                Status = model.Status,
                OrderDate = model.OrderDate,
                ClientPhone = model.ClientPhone,
                Email = model.Email,
                ClientName = model.ClientName,
                City = model.City,
                NovaPoshta = model.NovaPoshta,
                CartItemOrders = model.CartItemIds.Select(i => new CartItemOrder { CartItemId = i, OrderId = model.Id }).ToList(),

            };
        }
        public static OrderViewModel GetOrderById(Guid id, IOrderRepository repository)
        {
            return (repository.AllItems as DbSet<Order>)
                .Where(o => o.Id == id)
                .Include(o => o.CartItems)
                .Select(o => new OrderViewModel(o))
                .FirstOrDefault();
        }

        public static IQueryable<OrderViewModel> GetOrderList(IOrderRepository repository,
            Guid? categoryId = null)
        {
            
            return (repository.AllItems as DbSet<Order>)
                .Include(o=>o.CartItems)
                .Select(o => new OrderViewModel(o));
        }


    }
}
