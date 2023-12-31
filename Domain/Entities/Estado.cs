﻿using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Estado : BaseEntity
{
    

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}
