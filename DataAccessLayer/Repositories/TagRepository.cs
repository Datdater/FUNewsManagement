using BusinessLogicLayer.Contracts;
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
	public class TagRepository : GenericRepository<Tag>, ITagRepository
	{
		public TagRepository(NewsManagementDB context) : base(context)
		{
		}
	}
}
