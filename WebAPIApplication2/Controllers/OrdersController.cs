using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIApplication2.Data;
using WebAPIApplication2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
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
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
