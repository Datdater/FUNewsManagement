using DataAccessLayer.DatabaseConfiguration;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public class NewsTagRepository : GenericRepository<NewsTag>, INewsTagRepository
	{
		public NewsTagRepository(NewsManagementDB context) : base(context)
		{
		}
	}
}
