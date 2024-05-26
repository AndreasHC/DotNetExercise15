using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.Dto
{
    public class TournamentDto
    {

        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate => StartDate.AddMonths(3);

        // I feel ridiculous
        public void apply_back(Tournament tournament)
        {
            tournament.Title = Title;
            tournament.StartDate = StartDate;
        }
    }
}
