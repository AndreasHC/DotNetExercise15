using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private TournamentAPIApiContext _context;

        public TournamentRepository(TournamentAPIApiContext context)
        {
            _context = context;
        }

        public void Add(Tournament tournament)
        {
           _context.Add(tournament);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Tournament.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync(bool showGames = false)
        {
            IQueryable<Tournament> q = _context.Tournament;
            if ( showGames)
            {
                q = q.Include(t => t.Games);
            }
            return await q.ToListAsync();
        }

        public async Task<Tournament> GetAsync(int id)
        {
            return await _context.Tournament.FindAsync(id);
        }

        public void Remove(Tournament tournament)
        {
            _context.Tournament.Remove(tournament);
        }

        public void Update(Tournament tournament)
        {
            _context.Entry(tournament).State = EntityState.Modified;
        }
    }
}
