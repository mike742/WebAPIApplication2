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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public ActionResult Get()
        {
            var list = _context.Products.ToList();
            
            return Ok(list); // 200
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        // POST api/<ProductsController>
        [HttpPost]
        public ActionResult Post(ProductCreateDto value)
        {
            Product newProduct = _mapper.Map<Product>(value);

            _context.Products.Add(newProduct);
            _context.SaveChanges();

            return Ok(newProduct);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, ProductCreateDto value)
        {
            Product productFromDb = _context
                                        .Products
                                        .FirstOrDefault(p => p.Id == id);

            if (productFromDb == null) return NotFound();

            _mapper.Map(value, productFromDb);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Product productFromDb = _context
                            .Products
                            .FirstOrDefault(p => p.Id == id);

            if (productFromDb == null) return NotFound();

            _context.Products.Remove(productFromDb);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
