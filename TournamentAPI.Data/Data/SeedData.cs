using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data
{
    public class SeedData
    {
        private static Faker faker;
        public static async Task InitAsync(TournamentAPIApiContext context)
        {
            if (await context.Tournament.AnyAsync()) return;

            faker = new Faker("sv");

            var tournaments = GenerateTournaments(10);
            await context.AddRangeAsync(tournaments);

            var games = GenerateGames(tournaments);
            await context.AddRangeAsync(games);
            await context.SaveChangesAsync();
        }

        private static IEnumerable<Tournament> GenerateTournaments(int numberOfTournaments)
        {
            var rnd = new Random();
            var tournaments = new List<Tournament>();
            for (int i = 0; i < numberOfTournaments; i++)
            {
                string title = faker.Company.CatchPhrase();
                DateOnly startDate;
                if (rnd.Next(2) == 0)
                    startDate = faker.Date.SoonDateOnly();
                else
                    startDate = faker.Date.RecentDateOnly();
                Tournament tournament = new Tournament()
                {
                    Title = title,
                    StartDate = startDate.ToDateTime(TimeOnly.MinValue)
                };
                tournaments.Add(tournament);
            }

            return tournaments;
        }

        private static IEnumerable<Game> GenerateGames(IEnumerable<Tournament> tournaments)
        {
            var rnd = new Random();
            var games = new List<Game>();
            foreach (Tournament tournament in tournaments)
            {
                int numberOfGames = rnd.Next(1, 10);
                for (int i = 0;i < numberOfGames; i++)
                {
                    Game game = new Game()
                    {
                        Tournament = tournament,
                        time = faker.Date.Between(tournament.StartDate, tournament.StartDate.AddMonths(3)),
                        Title = faker.Commerce.ProductName()
                    };
                    games.Add(game);
                }
            }

            return games;
        }

    }
}
