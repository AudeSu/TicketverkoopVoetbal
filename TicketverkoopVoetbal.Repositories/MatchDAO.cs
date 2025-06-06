﻿using Microsoft.EntityFrameworkCore;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;

namespace TicketverkoopVoetbal.Repositories
{
    public class MatchDAO : IMatchDAO<Match>
    {
        private readonly FootballDbContext _dbContext;

        public MatchDAO(FootballDbContext context)
        {
            _dbContext = context;
        }

        public async Task Add(Match entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Match entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Match>?> FilterById(int id)
        {
            try
            {
                return await _dbContext.Matches
                .Where(a => a.ThuisploegId == id || a.UitploegId == id)
                .Include(a => a.Thuisploeg)
                .Include(a => a.Uitploeg)
                 .Include(b => b.Stadion)

                 .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public async Task<Match?> FindById(int id)
        {
            try
            {
                return await _dbContext.Matches
                .Where(a => a.MatchId == id)
                .Include(a => a.Thuisploeg)
                .Include(a => a.Uitploeg)
                 .Include(b => b.Stadion)

                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public async Task<IEnumerable<Match>?> FindByTwoIds(int thuisploegId, int uitploegId)
        {
            try
            {
                return await _dbContext.Matches
                .Where(a => a.ThuisploegId == thuisploegId && a.UitploegId == uitploegId  || a.UitploegId == thuisploegId && a.ThuisploegId == uitploegId)
                .Include(a => a.Thuisploeg)
                .Include(a => a.Uitploeg)
                 .Include(b => b.Stadion)

                 .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public async Task<IEnumerable<Match>?> GetAll()
        {
            try
            {
                return await _dbContext.Matches
                    .Include(b => b.Thuisploeg)
                    .Include(b => b.Uitploeg)
                    .Include(b => b.Stadion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public async Task Update(Match entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Match>?>FindByHomeClub(int thuisploegId)
        {
            try
            {
                return await _dbContext.Matches
                .Where(a => a.ThuisploegId == thuisploegId)
                .Include(a => a.Thuisploeg)
                .Include(a => a.Uitploeg)
                 .Include(b => b.Stadion)

                 .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }
    }
}
