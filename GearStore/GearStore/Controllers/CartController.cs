using GearStore.Controllers.CustomAttributes;
using GearStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GearStore.Controllers
{
    public class CartController : Controller
    {
        private ElectronicComponentsSMEntities _dataContext = new ElectronicComponentsSMEntities();
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["Cart"] is CartViewModel cart && cart.Items.Count > 0)
            {
                return View(cart);
            }
            return RedirectToAction("Index", "Products");
        }
        [HttpPost]
        public ActionResult AddToCart(int? id, int? quantity, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var obj = _dataContext.Products.SingleOrDefault(p => p.ProductID == id && !p.Discontinued);
            if (obj == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            quantity = quantity.HasValue ? quantity > 1 ? quantity : 1 : 1;
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new CartViewModel();
            }
            var cart = Session["Cart"] as CartViewModel;
            var item = cart.Items.Find(p => p.ProductID == id);
            if (item != null)
            {
                item.Quantity += quantity.Value;
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductID = obj.ProductID,
                    ProductName = obj.ProductName,
                    PhotoFilePatch = obj.PhotoFilePatch,
                    Price = obj.Price,
                    Quantity = quantity.Value,
                    Category = obj.Category,
                    Menu = obj.Category.Menu,
                    Manufacturer = obj.Manufacturer
                });
            }
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Products");
            }
            return Redirect(returnUrl);
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["Cart"] is CartViewModel cart)
            {
                cart.Items.Remove(cart.Items.Find(p => p.ProductID == id));
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction("Index", "Products");
                }
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Products");
        }
        public ActionResult RemoveFromMyCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["Cart"] is CartViewModel cart)
            {
                cart.Items.Remove(cart.Items.Find(p => p.ProductID == id));
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Products");
        }
        [HttpPost]
        public ActionResult UpdateCart(FormCollection form)
        {
            if (Session["Cart"] is CartViewModel cart)
            {
                int updateQuantity;
                foreach (var item in cart.Items)
                {
                    if (int.TryParse(form["product-" + item.ProductID], out updateQuantity) && updateQuantity >= 1)
                    {
                        item.Quantity = updateQuantity;
                    }
                    else
                    {
                        item.Quantity = 1;
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Products");
        }
        [UserAuthorize]
        public ActionResult Checkout()
        {
            if (Session["Cart"] is CartViewModel cart && cart.Items.Count > 0)
            {
                var account = Request.Cookies["Account"];
                var customerID = int.Parse(account["ID"]);
                ViewBag.Account = _dataContext.Customers.Find(customerID);
                Session["CheckoutCart"] = cart;
                return View(cart);
            }
            return RedirectToAction("Index", "Products");
        }
        [UserAuthorize,ActionName("Checkout")]
        [HttpPost]
        public async Task<ActionResult> SubmitCheckout()
        {
            if (Session["CheckoutCart"] is CartViewModel cart && cart.Items.Count > 0)
            {
                var account = Request.Cookies["Account"];
                var customerID = int.Parse(account["ID"]);
                using (var dbCxtTransaction = _dataContext.Database.BeginTransaction())
                {
                    string message = "";
                    try
                    {
                        var order = new Order
                        {
                            CustomerID = customerID,
                            OrderDate = DateTime.Now,
                            ShippedDate = DateTime.Now.AddDays(3),
                        };
                        _dataContext.Orders.Add(order);
                        await _dataContext.SaveChangesAsync();
                        foreach (var item in cart.Items)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderID = order.OrderID,
                                ProductID = item.ProductID,
                                Quantity = item.Quantity,
                                Price = item.Price,
                            };
                            _dataContext.OrderDetails.Add(orderDetail);
                            var product = _dataContext.Products.Find(item.ProductID);
                            product.UnitsInStock -= item.Quantity;
                            message = "Sản phẩm " + item.ProductName + " bị lỗi trong quá trình đặt hàng, xóa hoặc chỉnh sửa lại số lượng.";
                            await _dataContext.SaveChangesAsync();
                        }
                        dbCxtTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbCxtTransaction.Rollback();
                        ViewBag.Message = message;
                        return View(cart);
                    }
                }
                Session["Cart"] = null;
                Session["CheckoutCart"] = null;
                return RedirectToAction("IsCheckout");
            }
            return RedirectToAction("Index");
        }

        public ActionResult IsCheckout()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}