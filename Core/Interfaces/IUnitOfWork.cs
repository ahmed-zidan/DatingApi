﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IPhotoRepo _photo { get;}
        Task<bool> SaveChangesAsync();
    }
}
