using AutoMapper;
using BusinessLogicLayer.Model.AccountModel;
using BusinessLogicLayer.Model.CategoryModel;
using BusinessLogicLayer.Model.NewsArticleModel;
using BusinessLogicLayer.Model.TagModel;
using Microsoft.AspNetCore.Identity.Data;
using PresentationLayer.Models;
using PresentationLayer.Models.AccountViewModels;
using PresentationLayer.Models.CategoryViewModels;
using PresentationLayer.Models.NewsArticleModel;
using PresentationLayer.Models.TagViewModels;

namespace PresentationLayer.MapperProfile
{
    public class MapperModel : Profile
    {
        public MapperModel() 
        {
            //Login
            CreateMap<LoginViewModel, LoginDTO>();

            //map account
            CreateMap<AccountDTO, AccountViewModel>().ReverseMap();
            CreateMap<AccountCreateViewModel, AccountCreateDTO>().ReverseMap();
            CreateMap<AccountUpdateDTO, AccountUpdateViewModel>().ReverseMap();
            CreateMap<AccountDTO, AccountUpdateViewModel>().ReverseMap();

            //map tag
            CreateMap<TagViewModel, TagDTO>().ReverseMap();
            CreateMap<TagUpdateViewModel, TagDTO>().ReverseMap();
            CreateMap<TagUpdateViewModel, TagUpdateDTO>().ReverseMap();
            CreateMap<TagCreateViewModel, TagCreateDTO>().ReverseMap();

            //map news article
            CreateMap<NewsArticleDTO, NewsArticleViewModel>()
                .ReverseMap();
            CreateMap<NewsArticleCreateViewModel, NewsArticleCreateDTO>().ReverseMap();
            CreateMap<NewsArticleUpdateViewModel, NewsArticleUpdateDTO>().ReverseMap();
            CreateMap<NewsArticleUpdateViewModel, NewsArticleDTO>().ReverseMap();

            //Map category
            CreateMap<CategoryDTO, CategoryViewModel>().ReverseMap();
            CreateMap<CategoryUpdateViewModel, CategoryUpdateDTO>().ReverseMap();
            CreateMap<CategoryUpdateViewModel, CategoryDTO>().ReverseMap();
            CreateMap<CategoryCreateDTO, CategoryCreateViewModel>().ReverseMap();
        }
    }
}
