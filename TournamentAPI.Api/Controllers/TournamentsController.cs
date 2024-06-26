﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using Bogus.DataSets;
using AutoMapper;
using TournamentAPI.Core.Dto;
using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        //private readonly TournamentAPIApiContext _context;
        private readonly IUoW _uoW;
        private readonly IMapper _mapper;

        public TournamentsController(IUoW uoW, IMapper mapper)
        {
            _uoW = uoW;
            _mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournament(bool showGames = false)
        {
            return Ok(new ActionResult<IEnumerable<Tournament>>(await _uoW.TournamentRepository.GetAllAsync(showGames)));
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {
            var tournament = await _uoW.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }


            return Ok(_mapper.Map<Tournament, TournamentDto>(tournament));
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest();
            }

            _uoW.TournamentRepository.Update(tournament);

            try
            {
                await _uoW.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentExists(id))
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

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournament(Tournament tournament)
        {
            _uoW.TournamentRepository.Add(tournament);
            await _uoW.CompleteAsync();

            return CreatedAtAction("GetTournament", new { id = tournament.Id }, _mapper.Map<Tournament, TournamentDto>(tournament));
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _uoW.TournamentRepository.GetAsync(id); ;
            if (tournament == null)
            {
                return NotFound();
            }

            _uoW.TournamentRepository.Remove(tournament);
            await _uoW.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{TournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId, JsonPatchDocument<TournamentDto> patchDocument)
        {
            Tournament tournament = await _uoW.TournamentRepository.GetAsync(tournamentId);
            if (tournament == null)
                return NotFound();
            TournamentDto tournamentDto = _mapper.Map<Tournament, TournamentDto>(tournament);
            patchDocument.ApplyTo(tournamentDto);
            tournamentDto.apply_back(tournament);
            try
            {
                await _uoW.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }

            return Ok(_mapper.Map<Tournament, TournamentDto>(tournament));

        }

        private async Task<bool> TournamentExists(int id)
        {
            return await _uoW.TournamentRepository.AnyAsync(id);
        }
    }
}
