﻿using BulkyBook.Data.DBContext;
using BulkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Data.Repository
{
    public class CompanyRepository : GenericRepository<Company>
    {
        private readonly ApplicationDBContext context;
        public CompanyRepository(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }
    }
}
