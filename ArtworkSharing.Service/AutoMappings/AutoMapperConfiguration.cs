using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.Core.ViewModels.Artists;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Core.ViewModels.Artworks;
using ArtworkSharing.Core.ViewModels.Categories;
using ArtworkSharing.Core.ViewModels.Comments;
using ArtworkSharing.Core.ViewModels.Likes;
using ArtworkSharing.Core.ViewModels.MediaContent;
using ArtworkSharing.Core.ViewModels.Package;
using ArtworkSharing.Core.ViewModels.Rating;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Core.ViewModels.User;
using ArtworkSharing.Core.ViewModels.Users;
using ArtworkSharing.Core.ViewModels.VNPAYS;
using ArtworkSharing.Service.Services;
using AutoMapper;
using ArtworkService = ArtworkSharing.Core.Domain.Entities.ArtworkService;
using UserViewModel = ArtworkSharing.Core.ViewModels.User.UserViewModel;

namespace ArtworkSharing.Service.AutoMappings;

public class AutoMapperConfiguration
{
    private static readonly Lazy<IMapper> Lazy = new(() =>
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
        CreateMap<Transaction, TransactionsViewModelUser>().ReverseMap();
        CreateMap<Transaction, OrderViewModel>().ReverseMap();

        CreateMap<RefundRequest, RefundRequestViewModel>().ReverseMap();
        CreateMap<RefundRequest, UpdateRefundRequestModel>().ReverseMap();
        CreateMap<RefundRequest, CreateRefundRequestModel>().ReverseMap();

        CreateMap<ArtworkService, CreateArtworkRequestModel>().ReverseMap();
        CreateMap<ArtworkService, UpdateArtworkRequestModel>().ReverseMap();
        CreateMap<ArtworkService, ArtworkRequestViewModel>().ReverseMap();

        CreateMap<User, CreateUserViewModel>().ReverseMap();
        CreateMap<User, UserToLoginDto>().ReverseMap();
        CreateMap<User, UserToRegisterDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UpdateUserModelAdmin>().ReverseMap();

        CreateMap<Artwork, ArtworkCreateModelAdmin>().ReverseMap();
        CreateMap<Artwork, UpdateArtworkRequestModel>().ReverseMap();
        CreateMap<Artwork, ArtworkViewModelAdmin>().ReverseMap();

        
        CreateMap<RefundRequest, RefundRequestViewModel>().ReverseMap();
        CreateMap<RefundRequest, UpdateRefundRequestModel>().ReverseMap();
        CreateMap<RefundRequest, RefundRequestViewModelUser>().ReverseMap();
        
        
        CreateMap<MediaContent, MediaContentViewModel>().ReverseMap();
        CreateMap<Package, PackageViewModel>().ReverseMap();
        CreateMap<Rating, RatingViewModel>().ReverseMap();

        CreateMap<Artwork, ArtworkViewModel>().ReverseMap();
        CreateMap<Category, CategoryViewModel>().ReverseMap();
        CreateMap<Artist, ArtistViewModel>().ReverseMap();
        CreateMap<MediaContent, Core.ViewModels.MediaContents.MediaContentViewModel>().ReverseMap();

        CreateMap<User, UserViewModel>().ReverseMap();
        CreateMap<User, UserViewModel>().ReverseMap();
        CreateMap<User, Core.ViewModels.Users.UserViewModel>();
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

        CreateMap<VNPayTransaction, VNPayTransactionViewModel>().ReverseMap();
        CreateMap<ITransactionService, TransactionService>().ReverseMap();

    }
}