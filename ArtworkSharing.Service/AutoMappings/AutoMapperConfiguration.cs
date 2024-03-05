using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.Artists;
using ArtworkSharing.Core.ViewModels.Artworks;
using ArtworkSharing.Core.ViewModels.Categories;
using ArtworkSharing.Core.ViewModels.Comments;
using ArtworkSharing.Core.ViewModels.Likes;
using ArtworkSharing.Core.ViewModels.MediaContents;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Core.ViewModels.Users;
using AutoMapper;

namespace ArtworkSharing.Service.AutoMappings
{
    public class AutoMapperConfiguration
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cf =>
            {
                cf.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod!.IsAssembly;
                cf.AddProfile<MapperHandler>();
            });
            return config.CreateMapper();
        });

        public static IMapper Mapper => Lazy.Value;
    }

    public class MapperHandler : Profile
    {
        public MapperHandler()
        {
            CreateMap<Transaction, TransactionViewModel>().ReverseMap();
            CreateMap<Transaction, UpdateTransactionModel>().ReverseMap();

            CreateMap<RefundRequest, RefundRequestViewModel>().ReverseMap();
            CreateMap<RefundRequest, UpdateRefundRequestModel>().ReverseMap();

            CreateMap<Artwork, ArtworkViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Artist, ArtistViewModel>().ReverseMap();
            CreateMap<MediaContent, MediaContentViewModel>().ReverseMap();

            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, UpdateUserModel>().ReverseMap();
            CreateMap<User, CreateUserModel>().ReverseMap();
            CreateMap<User, UserToLoginDto>().ReverseMap();
            CreateMap<User, UserToRegisterDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Like, LikeModel>().ReverseMap();
            CreateMap<Like, LikeViewModel>().ReverseMap();

            CreateMap<Comment, CommentViewModel>().ReverseMap();
            CreateMap<Comment, CreateCommentModel>().ReverseMap();
            CreateMap<Comment, UpdateCommentModel>().ReverseMap();
        }
    }
}
