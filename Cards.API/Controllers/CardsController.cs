using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;
        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }

        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //Get Single Card
        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] int id)
        {
            var cards = await cardsDbContext.Cards.FindAsync(id);
            if (cards != null)
            {
                return Ok(cards);
            }
            return NotFound("card id : '" + id + "' Not Found !!!");
        }

        //Add card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card obj)
        {
            await cardsDbContext.Cards.AddAsync(obj);
            await cardsDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCard),new { id = obj.Id }, obj);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCard([FromRoute] int id, [FromBody] Card obj)
        {
            var cardToUpdate = await cardsDbContext.Cards.FindAsync(id);
            if (cardToUpdate != null)
            {
                cardToUpdate.CardHolderName = obj.CardHolderName;
                cardToUpdate.CardNumber = obj.CardNumber;
                cardToUpdate.ExpiryMonth = obj.ExpiryMonth;
                cardToUpdate.ExpiryYear = obj.ExpiryYear;
                cardToUpdate.CVC = obj.CVC;
                await cardsDbContext.SaveChangesAsync();
                return Ok(cardToUpdate);
            }
            return NotFound("Id : '" + id + "' Card Not Found !!!");
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCard([FromRoute] int id)
        {
            var cardToDelete= await cardsDbContext.Cards.FindAsync(id);
            if (cardToDelete != null)
            {
                cardsDbContext.Remove(cardToDelete);
                await cardsDbContext.SaveChangesAsync();
                return Ok(cardToDelete);
            }
            return NotFound("Id : '" + id + "' Card Not Found !!!");
        }
    }
}
