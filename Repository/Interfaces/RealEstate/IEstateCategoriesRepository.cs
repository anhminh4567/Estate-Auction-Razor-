﻿using Repository.Database.Model.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.RealEstate
{
	public interface IEstateCategoriesRepository : ICrud<EstateCategories>
	{
		Task<List<EstateCategories>> GetByEstateId(int id);
		Task<List<EstateCategories>> GetByCategoryId(int categoryId);
	}
}
