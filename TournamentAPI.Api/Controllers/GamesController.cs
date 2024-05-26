using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using TournamentAPI.Data.Repositories;
using TournamentAPI.Core.Repositories;
using AutoMapper;
using TournamentAPI.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUoW _uoW;
        private readonly IMapper _mapper;
        public GamesController(IUoW uoW, IMapper mapper)
        {
            _uoW = uoW;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame(string search = "")
        {
            return Ok(new ActionResult<IEnumerable<Game>>(await _uoW.GameRepository.GetAllAsync(search)));
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _uoW.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Game,GameDto>( game));
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest("Id mismatch");
            }

            _uoW.GameRepository.Update(game);

            try
            {
                await _uoW.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(Game game)
        {
            _uoW.GameRepository.Add(game);
            await _uoW.CompleteAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, _mapper.Map<Game, GameDto>(game));
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _uoW.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _uoW.GameRepository.Remove(game);
            await _uoW.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{GameId}")]
        public async Task<ActionResult<GameDto>> PatchGame(int gameId, JsonPatchDocument<GameDto> patchDocument)
        {
            Game game = await _uoW.GameRepository.GetAsync(gameId);
            if (game == null)
                return NotFound();
            GameDto gameDto = _mapper.Map<Game, GameDto>(game);
            patchDocument.ApplyTo(gameDto);
            gameDto.apply_back(game);
            try
            {
                await _uoW.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }

            return Ok(_mapper.Map<Game, GameDto>(game));

        }


        private async Task<bool> GameExists(int id)
        {
            return await _uoW.GameRepository.AnyAsync(id);
        }
    }
}
