﻿using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Repositories;

public class MediaContentRepository : Repository<MediaContent>, IMediaContentRepository
{
    public MediaContentRepository(DbContext dbContext) : base(dbContext)
    {
    }
}