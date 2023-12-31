﻿using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repository;

public class EstadoRepository : GenericRepository<Estado>, IEstado
{
        private readonly ApiContext _context;
        public EstadoRepository(ApiContext context) : base(context)
        {
                _context = context;
        }
}
