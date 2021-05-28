using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CriarCartaoApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CriarCartaoApi.Controllers
{
    [Route("api/Cartoes")]
    [ApiController]
    public class CartoesController : ControllerBase
    {
        private readonly CartaoContext _context;

        public CartoesController(CartaoContext context)
        {
            _context = context;
        }

        // GET: api/Cartoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cartao>>> GetCartoes()
        {
            return await _context.Cartoes.ToListAsync();
        }

        // GET: api/Cartoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cartao>> GetCartao(long id)
        {
            var cartao = await _context.Cartoes.FindAsync(id);

            if (cartao == null)
            {
                return NotFound();
            }

            return cartao;
        }

        [HttpGet("pesquisa/{email}")]
        public async Task<ActionResult<Cartao>> GetbyEmail(string email)
        {
            var contas = from c in _context.Cartoes
                        select c;
            if (!String.IsNullOrEmpty(email))
            {
                contas = contas.Where(c => c.Email.Equals(email));
                contas = contas.OrderBy(c => Convert.ToDateTime(c.DataSolicita));
            }

            if (contas == null)
            {
                return NotFound();
            }

            var cartoesUsuario = await contas.ToListAsync();
            return CreatedAtAction(nameof(GetbyEmail), cartoesUsuario);
        } 

        // PUT: api/Cartoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartao(long id, Cartao cartao)
        {
            if (id != cartao.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cartoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cartao>> PostCartao(Cartao cartao)
        {
            //Código que gera o número do cartão com 16 dígitos para salvar no BD
            string newCardNumber = "";
            Random randNum = new Random();
            for (int i = 0; i < 16; i++)
            {
                int digito = randNum.Next(0,9);
                newCardNumber += digito;
            }
            cartao.CardNumber = newCardNumber;

            //Buscando a data e hora do momento da criação do cartão para salvar no BD
            string data = DateTime.Now.ToString();
            cartao.DataSolicita = data;
            
            _context.Cartoes.Add(cartao);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCartao), new { id = cartao.Id }, cartao.CardNumber);
        }

        // DELETE: api/Cartoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartao(long id)
        {
            var cartao = await _context.Cartoes.FindAsync(id);
            if (cartao == null)
            {
                return NotFound();
            }

            _context.Cartoes.Remove(cartao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartaoExists(long id)
        {
            return _context.Cartoes.Any(e => e.Id == id);
        }
    }
}
