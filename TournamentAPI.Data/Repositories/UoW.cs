using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class UoW : IUoW
    {
        private TournamentAPIApiContext _context;
        public UoW(TournamentAPIApiContext context)
        {
            TournamentRepository = new TournamentRepository(context);
            GameRepository = new GameRepository(context);
            _context = context;
        }

        public ITournamentRepository TournamentRepository { get; init; }

        public IGameRepository GameRepository { get; init; }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
