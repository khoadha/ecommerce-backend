using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.BiddingRepository;
using DataAccessLayer.Repositories.CartRepository;
using DataAccessLayer.Repositories.CategoryMaterialRepository;
using DataAccessLayer.Repositories.ChatRepository;
using DataAccessLayer.Repositories.FeedbackRepository;
using DataAccessLayer.Repositories.GlobalSettingRepository;
using DataAccessLayer.Repositories.PaymentTransactionRepository;
using DataAccessLayer.Repositories.ReportRepository;
using DataAccessLayer.Repositories.VoucherRepository;
using EXE_API.Services.ApplicationUserService;
using EXE_API.Services.CartService;
using EXE_API.Services.OrderService;
using EXE_API.Services.ProductService;
using EXE_API.Services.StatisticService;
using EXE_API.Services.StoreService;
using EXE101_API.Context;
using EXE101_API.Services.BiddingService;
using EXE101_API.Services.CacheService;
using EXE101_API.Services.CategoryMaterialService;
using EXE101_API.Services.ChatService;
using EXE101_API.Services.EmailService;
using EXE101_API.Services.FeedbackService;
using EXE101_API.Services.GlobalSettingService;
using EXE101_API.Services.ReportService;
using EXE101_API.Services.VnPayService;
using EXE101_API.Services.VoucherService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace EXE_API.Extensions {
    public static class ServiceExtensions {

        public static void ConfigureDILifeTime(this IServiceCollection services) {
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<ICategoryMaterialService, CategoryMaterialService>();
            services.AddScoped<IBiddingService, BiddingService>();
            services.AddScoped<IGlobalSettingService, GlobalSettingService>();
            services.AddScoped<IBiddingService, BiddingService>();
            services.AddScoped<IChatService,ChatService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IFeedbackService, FeedbackService>();


            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<ICategoryMaterialRepository, CategoryMaterialRepository>();
            services.AddScoped<IBiddingRepository, BiddingRepository>();
            services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
            services.AddScoped<IGlobalSettingRepository, GlobalSettingRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IFeedbackRepository,  FeedbackRepository>();

            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IVnPayService, VnPayService>();
        }

        public static void ConfigureCors(this IServiceCollection services) {
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                         .WithOrigins(new string[] { "https://localhost:4200", "http://localhost:4200", "https://wemade.azurewebsites.net", "https://wemade.site" })
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .SetIsOriginAllowed(origin => true)
                         .AllowCredentials());
            });
        }

        public static void ConfigureSwaggerGen(this IServiceCollection services) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EXEApp", Version = "v1" });
                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services, byte[] key) {
            var tokenValidationParams = new TokenValidationParameters() {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParams);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(jwt => {
                        jwt.SaveToken = true;
                        jwt.RequireHttpsMetadata = true;
                        jwt.TokenValidationParameters = tokenValidationParams;
                    });
        }

    }
}
