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
    public class GameRepository : IGameRepository
    {
        private TournamentAPIApiContext _context;

        public GameRepository(TournamentAPIApiContext context)
        {
            _context = context;
        }

        public void Add(Game game)
        {
           _context.Add(game);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Game.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllAsync(string search)
        {
            IQueryable<Game> q = _context.Game;
            if (!string.IsNullOrEmpty(search))
            {
                q = q.Where(g => g.Title == search);
            }
            return await q.ToListAsync();
        }

        public async Task<Game> GetAsync(int id)
        {
            return await _context.Game.FindAsync(id);
        }

        public void Remove(Game game)
        {
            _context.Game.Remove(game);
        }

        public void Update(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
        }
    }
}
