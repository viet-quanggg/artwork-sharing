using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.ArtworkRequest;
using ArtworkSharing.Core.Domain.Dtos.UserDtos;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.ViewModels.RefundRequests;
using ArtworkSharing.Core.ViewModels.Transactions;
using ArtworkSharing.Core.ViewModels.User;
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
            CreateMap<RefundRequest, CreateRefundRequestModel>().ReverseMap();
            CreateMap<ArtworkService, CreateArtworkRequestModel>().ReverseMap();
            CreateMap<ArtworkService, UpdateArtworkRequestModel>().ReverseMap();
            CreateMap<ArtworkService, ArtworkRequestViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, CreateUserViewModel>().ReverseMap();
            CreateMap<User, UserToLoginDto>().ReverseMap();
            CreateMap<User, UserToRegisterDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
