﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationalRental.Domain.Entities;
using VacationalRental.Domain.Interfaces.Repositories;
using VacationalRental.Infrastructure.DbContexts;

namespace VacationalRental.Infrastructure.Repositories
{
    public class RentalsRepository : IRentalsRepository
    {
        private readonly VacationRentalDbContext _vacationalRentalDbContext;

        public RentalsRepository(VacationRentalDbContext vacationalRentalDbContext)
        {
            _vacationalRentalDbContext = vacationalRentalDbContext;
        }

        public async Task<int> GetRentalUnits(int rentalID)
        {
            var rentalUnits = await _vacationalRentalDbContext.RentalEntities.Where(a => a.Id == rentalID).Select(a => a.Units).ToListAsync();

            return rentalUnits.Single();
        }

        public async Task<int> GetRentalPreparationTimeInDays(int rentalID)
        {
            var preparationTimeInDays = await _vacationalRentalDbContext.RentalEntities.Where(a => a.Id == rentalID).Select(a => a.PreprationTimeInDays).ToListAsync();

            return preparationTimeInDays.Single();
        }

        public async Task<int> InsertNewRentalObtainRentalId(RentalEntity rentalEntity)
        {
            await _vacationalRentalDbContext.RentalEntities.AddAsync(rentalEntity);

            await _vacationalRentalDbContext.SaveChangesAsync();

            return rentalEntity.Id;
        }

        public async Task<RentalEntity> GetRentalById(int rentalId)
        {
            return await _vacationalRentalDbContext.RentalEntities.FindAsync(rentalId);
        }

        public async Task<bool> RentalExists(int rentalId)
        {
            return await _vacationalRentalDbContext.RentalEntities.AnyAsync(a => a.Id == rentalId);
        }
    }
}