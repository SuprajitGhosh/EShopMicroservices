using Discount.Grpc.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Discount.Grpc.Models;


namespace Discount.Grpc.Services
{
    public class DiscountServices(DiscountContext dbContext, ILogger<DiscountServices> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon is null)
                coupon = new Models.Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };

            logger.LogInformation("ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null) {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            }
            dbContext.coupons.Add(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("discount is created successfully. ProductName : {ProductName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);
            var couponmodel = coupon.Adapt<CouponModel>();
            return couponmodel;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            }
            dbContext.coupons.Update(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("discount is updated successfully. ProductName : {ProductName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);
            var couponmodel = coupon.Adapt<CouponModel>();
            return couponmodel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, "not found"));
            dbContext.coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("discount successfully deleted");

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
