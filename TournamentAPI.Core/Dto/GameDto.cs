using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Dto
{
    public class GameDto
    {

        public string Title { get; set; }
        public DateTime Time { get; set; }

        public void apply_back(Game game)
        {
            game.Title = Title;
            game.Time = Time;
        }
    }
}
