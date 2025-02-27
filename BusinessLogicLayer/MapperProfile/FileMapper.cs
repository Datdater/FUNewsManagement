using AutoMapper;
using BusinessLogicLayer.Model.AccountModel;
using BusinessLogicLayer.Model.CategoryModel;
using BusinessLogicLayer.Model.NewsArticleModel;
using BusinessLogicLayer.Model.TagModel;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.MapperProfile
{
	public class FileMapper : Profile
	{
		public FileMapper() 
		{
			//Account
			CreateMap<SystemAccount, AccountDTO>().ReverseMap();
            CreateMap<AccountUpdateDTO, SystemAccount>()
			.ForMember(dest => dest.AccountPassword, opt => opt.Ignore());	
			CreateMap<SystemAccount, AccountCreateDTO>().ReverseMap();

			//Tag
			CreateMap<TagDTO, Tag>().ReverseMap();
			CreateMap<Tag, TagCreateDTO>().ReverseMap();
			CreateMap<Tag, TagUpdateDTO>().ReverseMap();

			//News articles
			CreateMap<NewsArticle, NewsArticleDTO>()
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.AccountName))
                .ForMember(dest => dest.UpdatedByName, opt => opt.MapFrom(src => src.UpdatedBy.AccountName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
			CreateMap<NewsArticle, NewsArticleCreateDTO>().ReverseMap();
			CreateMap<NewsArticle, NewsArticleUpdateDTO>().ReverseMap();

			//Category
			CreateMap<Category, CategoryDTO>().ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory.CategoryName)).ReverseMap();
			CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
			CreateMap<Category, CategoryCreateDTO>().ReverseMap();
		}
	}
}
