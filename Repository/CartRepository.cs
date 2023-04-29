using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ASM.Repository
{
    public class CartRepository: ICartRepository 
    {
        private readonly ASMContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepository(ASMContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction(); //allow serveral databases operations to be processed in an atomic manner 
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        CustomerID = userId
                    };
                    //_db.Carts.Add(cart);
                }
                _db.SaveChanges();
                //cart detail 
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.CartID == cart.Id && a.ProductID == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _db.Product.Find(productId);
                    cartItem = new CartDetails
                    {
                        Cart = cart,
                        Product = product,
                        ProductID = productId,
                        CartID = cart.Id,
                        Quantity = qty,
                        Price = product.ProductPrice,
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                
                //If the transaction is committed, all of the operations are successfully applied to the database.
                //If the transaction is rolled back, none of the operations are applied to the database.
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("cart is empty");
                }
                // find cart details
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.CartID == cart.Id && a.ProductID == productId);
                if (cartItem is null)
                    throw new Exception("item is not in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null) 
            {
                throw new Exception("Invalid userid");
            }
            var cart = await _db.Carts
                .Include(a => a.CartDetails)
                .ThenInclude(cd => cd.Product)
                .Where(a => a.CustomerID == userId).FirstOrDefaultAsync();
            return cart;
        }
        public async Task<Cart> GetCart(string userId) 
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.CustomerID == userId);
            return cart;
        }
        public async Task<int> GetCartItemCount(string userId="") 
        {
            if (!string.IsNullOrEmpty(userId)) 
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.Carts
                        join cartDetail in _db.CartDetails
                        on cart.Id equals cartDetail.CartID
                        select new { cartDetail.id}
                        ).ToListAsync();
            return data.Count;
        }
        public async Task<bool> DoCheckOut() 
        {
            using var transaction = _db.Database.BeginTransaction();
            try 
            {
                //move data from cartdetail to order and order detail then we will remove cart detail
                //entry order
                var userId = GetUserId() ;
                if(string.IsNullOrEmpty(userId)) 
                {
                    throw new Exception("User not logged in");
                }
                var cart = await GetCart(userId);
                if (cart is null) 
                {
                    throw new Exception("Invalid cart!");
                }
                var cartDetail = await _db.CartDetails
                                    .Where(a => a.CartID == cart.Id).Include(od => od.Product).ToListAsync();
                if (cartDetail.Count == 0) 
                {
                    throw new Exception("Cart is empty");
                }
                //Get total price
                float price = 0;
                foreach (var a in cartDetail) 
                {
                    price += a.Price * a.Quantity;
                }
                var order = new Order
                {
                    CustomerId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatusID = 1, //pending
                    OrderTotalPrice = price,
                };
                _db.Order.Add(order);
                _db.SaveChanges();
                //Enty orderdetail
                foreach (var item in cartDetail) 
                {
                    var orderDetail = new OrderDetail
                    {
                        Order = order,
                        ProductId = item.ProductID,
                        Product = item.Product,
                        OrderId = order.OrderId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                    };
                    _db.OrderDetail.Add(orderDetail);
                }
                _db.SaveChanges();

                //remove data from cart detail
                _db.CartDetails.RemoveRange(cartDetail); //remove mulitple cartdetail => removerange
                _db.SaveChanges();
                transaction.Commit();
                return true;

            }
            catch (Exception ex) 
            {
                return false;
            }
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;

        }
    }
}
