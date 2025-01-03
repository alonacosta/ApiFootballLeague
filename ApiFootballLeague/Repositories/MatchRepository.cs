﻿using ApiFootballLeague.Models;
using ApiFootballLeague.ViewModels;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace ApiFootballLeague.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly LeagueDbContext _context;

        public MatchRepository(LeagueDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Round>> GetRoundsAsync()
        {
            return await _context.Rounds                
             .OrderByDescending(g => g.DateStart)
             .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetMatchesByRoundIdAsync(int roundId)
        {
            return await _context.Matches
                .Include(m => m.Round)                             
                .Where(p => p.RoundId == roundId)
                .ToListAsync();
        }

        public async Task<List<StatisticsViewModel>> CalculateStatisticsAsync()
        {
            var clubs = await _context.Clubs.ToListAsync();

            var matches = await _context.Matches  
                .Include(m => m.Round)
                 .ToListAsync();

            var statistics = matches
                .SelectMany(m => new[]
                {
                    new { Club = m.HomeTeam, Scored = m.HomeScore, Conceded = m.AwayScore, IsHome = true, IsFinished = m.IsFinished },
                    new { Club = m.AwayTeam, Scored = m.AwayScore, Conceded = m.HomeScore, IsHome = false, IsFinished = m.IsFinished},
                })
                .GroupBy(m => m.Club)
                .Select(g =>
                {
                    var club = clubs.FirstOrDefault(c => c.Name == g.Key);

                    return new StatisticsViewModel
                    {
                        ClubName = g.Key,
                        ImageId = club.ImageId,
                        ImageFullPath = club.ImageFullPath,
                        TotalMatches = g.Count(),
                        Wins = g.Count(x => (x.IsHome && x.Scored > x.Conceded) || (!x.IsHome && x.Scored > x.Conceded)),
                        Draws = g.Count(x => x.Scored == x.Conceded),
                        Losses = g.Count(x => (x.IsHome && x.Scored < x.Conceded) || (!x.IsHome && x.Scored < x.Conceded)),
                        GoalsScored = g.Sum(x => x.Scored),
                        GoalsConceded = g.Sum(x => x.Conceded),
                        Points = g.Sum(x => x.Scored > x.Conceded ? 3 : x.Scored == x.Conceded ? 1 : 0),
                        Finished = g.Count(x => x.IsFinished),
                        Scheduled = g.Count(x => !x.IsFinished),
                    };
                })
                .OrderByDescending(s => s.Points)
                .ToList();

            for (int i = 0; i < statistics.Count; i++)
            {
                statistics[i].Position = i + 1;
            }
            return statistics;
        }

        public async Task<Club> GetClubeByNameAsync(string ClubName)
        {
            return await _context.Clubs
                .Include(p => p.Players)
                .Where(c => c.Name == ClubName)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetImageClubUrl(string ClubName)
        {
            var club = await _context.Clubs
                .Include(p => p.Players)            
                .FirstOrDefaultAsync(c => c.Name == ClubName);

           return club.ImageId == Guid.Empty
            ? $"https://footballleague.blob.core.windows.net/default/no-image.jpeg" : $"https://footballleague.blob.core.windows.net/clubs/{club.ImageId}";
        }
    }
}
