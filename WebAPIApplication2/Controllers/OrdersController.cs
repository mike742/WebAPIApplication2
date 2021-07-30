using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIApplication2.Data;
using WebAPIApplication2.DTOs;
using WebAPIApplication2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrdersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public ActionResult Get()
        {
            /*
            var orders = _context.Orders.ToList();
            var product = _context.Products.ToList();
            var orderProducts = _context.OrderProducts.ToList();

            foreach (var order in orders)
            {
                List<Product> productsToAdd = new List<Product>();

                foreach (var op in orderProducts)
                {
                    if (op.OrderId == order.Id)
                    {
                        Product p = product
                            .FirstOrDefault(p => p.Id == op.ProductId);
                        productsToAdd.Add(p);
                    }
                }

                order.Products = productsToAdd;
            }
            */

            var orders = _context
                .Orders
                .Select(o => new Order
                {
                    Id = o.Id,
                    Name = o.Name,
                    Date = o.Date,
                    Products = _context.OrderProducts
                        .Where(op => op.OrderId == o.Id)
                        .Select(p => new Product
                        {
                            Id = p.Product.Id,
                            Name = p.Product.Name,
                            Price = p.Product.Price
                        }).ToList()
                }).ToList();

            return Ok(orders);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            /*
            Order order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound($"Order with id = {id} doesn't exist");
            }

            List<Product> productsToAdd = new List<Product>();

            foreach (var op in _context.OrderProducts.ToList())
            {
                if (op.OrderId == order.Id)
                {
                    Product p = _context
                            .Products
                            .FirstOrDefault(p => p.Id == op.ProductId);

                    productsToAdd.Add(p);
                }
            }

            order.Products = productsToAdd;
            */

            var order = _context.Orders.Where(o => o.Id == id)
                .Select(o => new Order
                {
                    Id = o.Id,
                    Name = o.Name,
                    Date = o.Date,
                    Products = _context.OrderProducts
                        .Where(op => op.OrderId == o.Id)
                        .Select(p => new Product
                        {
                            Id = p.Product.Id,
                            Name = p.Product.Name,
                            Price = p.Product.Price
                        }).ToList()
                }).FirstOrDefault();

            if (order == null) 
                return NotFound($"#2: Order with id = {id} doesn't exist");

            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public ActionResult Post(OrderCreateDto value)
        {
            Order newOrder = _mapper.Map<Order>(value);
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            foreach (int prodId in value.ProductIds)
            {
                var op = new OrderProducts { 
                    OrderId = newOrder.Id,
                    ProductId = prodId
                };

                _context.OrderProducts.Add(op);
            }

            _context.SaveChanges();

            return Ok();
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, OrderCreateDto value)
        {

            var orderFromDb = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (orderFromDb == null) return NotFound();

            _mapper.Map(value, orderFromDb);


            var productsRange = _context
                .OrderProducts
                .Where(op => op.OrderId == id)
                .ToList();

            _context.OrderProducts.RemoveRange(productsRange);

            foreach (var prodId in value.ProductIds)
            {
                var opNew = new OrderProducts { 
                    OrderId = id,
                    ProductId = prodId
                };

                _context.OrderProducts.Add(opNew);
            }

            _context.SaveChanges();

            return Ok();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Order orderToDelete = _context.Orders.Find(id);

            if (orderToDelete == null) return NotFound();

            var productsRange = _context
                  .OrderProducts
                  .Where(op => op.OrderId == id)
                  .ToList();

            _context.OrderProducts.RemoveRange(productsRange);
            _context.Orders.Remove(orderToDelete);
            _context.SaveChanges();

            return Ok();
        }
    }
}
