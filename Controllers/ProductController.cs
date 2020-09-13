using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.
                    Products.
                    Include(x => x.Category).
                    AsNoTracking().
                    ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Product>>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.
                    Products.
                    Include(x => x.Category).
                    AsNoTracking().
                    FirstOrDefaultAsync(x => x.Id == id);
            return Ok(product);
        }

        [HttpGet]
        [Route("{categories/id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory(
            [FromServices] DataContext context,
            int id
        )
        {
            var products = await context.
                    Products.
                    Include(x => x.Category).
                    AsNoTracking().
                    Where(x => x.CategoryId == id).
                    ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post(
            [FromServices] DataContext context,
            [FromBody] Product model
        )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Products.Add(model);
                    await context.SaveChangesAsync();
                    return Ok(model);
                }
                catch (System.Exception ex)
                {
                    return BadRequest(new { message = "Ocorreu um erro ao salvar o produto! Erro: " + ex.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}