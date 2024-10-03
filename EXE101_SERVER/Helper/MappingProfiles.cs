using AutoMapper;
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE101_Backend.Shared;

namespace EXE_API.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {

            //AUTH
            CreateMap<UserLoginRequestDto, ApplicationUser>();

            //STORE
            CreateMap<StoreRegistrationRequestDto, Store>();
            CreateMap<UpdateStoreDto, Store>();
            CreateMap<Store, GetStoreDto>()
                .ForMember(des => des.StoreEmail, act => act.MapFrom(src => src.Manager.Email));

            //PRODUCT
            CreateMap<AddProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, GetProductDto>()
                .ForMember(des => des.StoreImgPath, act => act.MapFrom(src => src.Store.ImgPath))
                .ForMember(des => des.StoreName, act => act.MapFrom(src => src.Store.Name))
                .ForMember(des => des.DistrictId, act => act.MapFrom(src => src.Store.DistrictId))
                .ForMember(des => des.WardCode, act => act.MapFrom(src => src.Store.WardCode));
            CreateMap<Product, GetProductDetailPageDto>()
                .ForMember(des => des.StoreImgPath, act => act.MapFrom(src => src.Store.ImgPath))
                .ForMember(des => des.StoreName, act => act.MapFrom(src => src.Store.Name))
                .ForMember(des => des.DistrictId, act => act.MapFrom(src => src.Store.DistrictId))
                .ForMember(des => des.WardCode, act => act.MapFrom(src => src.Store.WardCode)); ;
            CreateMap<UpdateProductImageDto, Product>();
            CreateMap<Product, UpdateProductImageDto> ();
            CreateMap<Product, GetProductImageDto>();
            CreateMap<GetProductImageDto, Product>();


            //USER
            CreateMap<ApplicationUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<GetPersonalUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetPersonalUserDto>();

            //Cart
            CreateMap<GetCartDto, Cart>();
            CreateMap<Cart, GetCartDto>();
            CreateMap<GetCartItemDto, CartItem>();
            CreateMap<CartItem, GetCartItemDto>();

            //VOUCHER
            CreateMap<GetVoucherDto, Voucher>();
            CreateMap<Voucher, GetVoucherDto>();
            CreateMap<AddVoucherDto, Voucher>();

            //BIDDING
            CreateMap<AddBiddingRequestDto, Bidding>();
            CreateMap<Bidding, GetBiddingDto>();
            CreateMap<Bidding, GetBiddingDetailDto>();

            //AUCTIONEER
            CreateMap<AddAuctioneerDto, Auctioneer>();
            CreateMap<Auctioneer, GetAuctioneersByBiddingDto>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.StoreImgPath, opt => opt.MapFrom(src => src.Store.ImgPath))
                .ForMember(dest => dest.ListImages, opt => opt.MapFrom(src => src.ListImages.Select(a=>a.Url).ToList()));

            //ORDER
            CreateMap<GetOrderDto, Order>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<Order, GetOrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<GetOrderByUserDto, Order>();
            CreateMap<GetOrderByStoreDto, Order>();
            CreateMap<Order, GetOrderByUserDto>();
            CreateMap<Order, GetOrderByStoreDto>();

            CreateMap<AddOrderProductsDto, OrderProducts>();
            CreateMap<AddOrderDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<GetOrderProductsDto, OrderProducts>();
            CreateMap<OrderProducts, GetOrderProductsDto>();

            //CATEGORY AND MATERIAL
            CreateMap<Category, GetCategoryDto>();
            CreateMap<Material, GetMaterialDto>();
            CreateMap<AddMaterialDto, Material>();
            CreateMap<AddCategoryDto, Category>();

            //STATISTIC
            CreateMap<TopProductStatistic, TopProductStatisticDto>();
            CreateMap<Product, ProductForTopStatisticDto>();

            //REPORT
            CreateMap<AddReportDto, Report>();

            //FEEDBACK
            CreateMap<AddFeedbackDto, Feedback>();
            CreateMap<Feedback, GetFeedbackDto>()
                .ForMember(dest => dest.ListImages, opt => opt.MapFrom(src => src.FeedbackImages.Select(a => a.Url).ToList()))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.User.ImgPath));
        }
    }
}
