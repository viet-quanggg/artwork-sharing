using System.Text;
using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Exceptions;
using ArtworkSharing.Core.Helpers.MsgQueues;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Repositories;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL;
using ArtworkSharing.DAL.Data;
using ArtworkSharing.Service.Services;
using ArtworkSharing.Service.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ArtworkService = ArtworkSharing.Service.Services.ArtworkService;

namespace ArtworkSharing.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///     Add needed instances for database
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<Func<ArtworkSharingContext>>(provider => () => provider.GetService<ArtworkSharingContext>());
        services.AddScoped<DbFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    /// <summary>
    ///     Add instances of in-use services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IArtworkService, ArtworkService>();
        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<IArtistPackageService, ArtistPackageService>();
        services.AddScoped<IRefundRequestService, RefundRequestService>();
        services.AddScoped<IArtworkRequestService, ArtworkRequestService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<UnitOfWork>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IFollowService, FollowService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IPackageService, PackageService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IFireBaseService, FireBaseService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IVNPayTransactionService, VNPayTransactionService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IPaymentEventService, PaymentEventService>();
        services.AddScoped<IVNPayTransactionTransferService, VNPayTransactionTransferService>();
        services.AddScoped<IPaypalOrderService, PaypalOrderService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPaymentRefundEventService, PaymentRefundEventService>();
        services.AddScoped<IPaypalPaymentEventService, PaypalPaymentEventService>();
        services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        services.AddScoped<IPaypalRefundEventService, PaypalRefundEventService>();


        services.AddTransient<IEmailSender, EmailSender>();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<UserToLoginDTOValidator>();
        services.AddValidatorsFromAssemblyContaining<UserToRegisterDTOValidator>();

        MessageChanel messageChanel = new();
        services.AddSingleton<MessageChanel>(messageChanel.PaidRaise());
        services.AddSingleton<MessageChanel>(messageChanel.PaypalPaidRaise());
        services.AddSingleton<MessageChanel>(messageChanel.RefundPaidRaise());
        services.AddSingleton<MessageChanel>(messageChanel.RefundPaypalPaidRaise());
        services.AddSingleton<IMessageSupport, MessageSupport>();
        services.AddSingleton<MessagePaymentEvent>();
        services.AddSingleton<MessageRefundEvent>();
        services.AddHostedService<MessagePaymentEvent>(_ => _.GetService<MessagePaymentEvent>()!);
        services.AddHostedService<MessageRefundEvent>(_ => _.GetService<MessageRefundEvent>()!);
        services.AddHostedService<MessageSubscribe>();
        services.AddHostedService<MessageRefundSubscribe>();
        return services;
    }


    public static void AddConfigException(this IServiceCollection services)
    {
        services.AddControllers(_ => { _.Filters.Add(new BusinessException()); });
    }

    public static void UseException(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoleManager<RoleManager<Role>>()
            .AddEntityFrameworkStores<ArtworkSharingContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(24); // Token expires after 24 hours
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
               

            })
            ;
        //services.AddAuthentication().AddGoogle(options =>
        //{
            
        //    // You can set other options as needed.
        //});
        
        
        //services.AddAuthorization(opt =>
        //{
        //    opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
        //    opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
        //});
        return services;
    }

    
}