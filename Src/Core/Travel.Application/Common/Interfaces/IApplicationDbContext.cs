using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travel.Domain.Entities;

namespace Travel.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TourList> TourLists { get; set; }

        DbSet<TourPackage> TourPackages { get; set; }

        DbSet<Todo> Todos { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
