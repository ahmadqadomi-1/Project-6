using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using project_6.Models;

namespace project_6.Controllers
{
    public class UsersController : Controller
    {
        private MiniProjectEntities db = new MiniProjectEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }


        public ActionResult TermsConditions()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }


        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Fname_User,Lname_User,User_Email,User_Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "User_Email,User_Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var user2 = db.Users.FirstOrDefault(guest => guest.User_Email == user.User_Email && guest.User_Password == user.User_Password);
                if (user2 != null)
                {
                    Session["guest"] = user2.UserID; 
                    return RedirectToAction("Index"); 
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(user); 
        }


     



        public ActionResult Item(int id)
        {
            var item = db.Products.Where(x => x.ID == id).FirstOrDefault();
            return View(item);
        }
        public ActionResult Shop(int id)
        {
            var product = db.Products.Where(x=>x.CategoryID==id).ToList();
            return View(product);
        }

        public ActionResult AddToCart(int id)
        {
            var num = (int)Session["guest"];
            var check = db.Carts.Where(model => model.UserID == num).FirstOrDefault();

            if (check == null)
            {
                var userCard = new Cart
                {
                    UserID = num,
                    CreateDate = DateTime.Now,
                };

                db.Carts.Add(userCard);
                db.SaveChanges();
            }


            var checkProduct = db.ShoppingCartItems.Where(model => model.ProductID == id).FirstOrDefault();

            if (checkProduct == null)
            {
                var check2 = db.Carts.Where(model => model.UserID == num).FirstOrDefault();

                var userCardItems = new ShoppingCartItem
                {
                    CartID = check2.CartID,
                    ProductID = id,
                    Quantity = 1,
                    CreateDate = DateTime.Now
                };
                db.ShoppingCartItems.Add(userCardItems);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                checkProduct.Quantity = checkProduct.Quantity + 1;
                db.Entry(checkProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


        }
        public ActionResult Cart()
        {
            var userId = Session["guest"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Index");
            }

            var shoppingCart = db.Carts.FirstOrDefault(m => m.UserID == userId);
            if (shoppingCart == null)
            {
                return RedirectToAction("Index");
            }

            var shoppingCartItems = db.ShoppingCartItems
                .Include(x => x.Product)
                .Where(m => m.CartID == shoppingCart.CartID)
                .ToList();

            var totalAmount = shoppingCartItems.Sum(item => item.Quantity * item.Product.Price);

            ViewBag.TotalAmount = totalAmount;

            return View(shoppingCartItems);
        }


        [HttpPost]
        public ActionResult UpdateQuantity(int id, string operation)
        {
            var item = db.ShoppingCartItems.Where(model => model.ProductID == id).FirstOrDefault();

            if (item != null)
            {
                if (operation == "increase")
                {
                    item.Quantity++;
                }
                else if (operation == "decrease" && item.Quantity > 1)
                {
                    item.Quantity--;
                }

                db.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult DeleteItem(int id)
        {
            var item = db.ShoppingCartItems.SingleOrDefault(i => i.ProductID == id);

            if (item != null)
            {
                db.ShoppingCartItems.Remove(item);
                db.SaveChanges();
            }

            return RedirectToAction("Cart");
        }


        public ActionResult ProfilePage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilePage([Bind(Include = "UserID,Fname_User,Lname_User,User_Email,User_Password")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing user from the database
                    var existingUser = db.Users.Find(user.UserID);
                    if (existingUser != null)
                    {
                        // Update only the fields you want to change
                        existingUser.Fname_User = user.Fname_User;
                        existingUser.Lname_User = user.Lname_User;
                        existingUser.User_Email = user.User_Email;

                        if (!string.IsNullOrEmpty(user.User_Password))
                        {
                            // Only update the password if provided
                            existingUser.User_Password = user.User_Password;
                        }

                        // Mark entity as modified and save changes
                        db.Entry(existingUser).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("ProfilePage", new { id = user.UserID });
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (you can implement logging here)
                    ModelState.AddModelError("", "An error occurred while updating the profile: " + ex.Message);
                }
            }

            return View(user); // Return view with the model if validation fails
        }



    }
}
