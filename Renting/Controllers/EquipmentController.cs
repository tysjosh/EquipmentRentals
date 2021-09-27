using System;
using System.Web.Mvc;
using Renting.Interface;
using Renting.Repository.Models;

namespace Renting.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _equipmentService;
       

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
          
        }

        /// <summary>
        /// Adds to cart.
        /// </summary>
        /// <param name="equipmentModel">The equipment model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(EquipmentModel equipmentModel)
        {
            if (!ModelState.IsValid)
            {
                var message = "An Error has occured in your input";
                return RedirectToAction("Index", "Home", new {message});
            }


            if (equipmentModel == null)
            {
                throw new ArgumentNullException(nameof(equipmentModel));
            }


            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Removes from cart.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">equipmentId</exception>
        [HttpPost]
        public ActionResult RemoveFromCart(int equipmentId)
        {
            if (equipmentId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(equipmentId));
            }


            _equipmentService.RemoveFromCart(equipmentId);
            ViewBag.Message = "Selected Equipment Deleted From Cart";

            return RedirectToAction("Cart", "Equipment");
        }

        /// <summary>
        /// Carts this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Cart(string message)
        {
            var model = _equipmentService.GetEquipmentCartModel(message);
            return View("Cart", model);
        }

        /// <summary>
        /// Searches the equipment.
        /// </summary>
        /// <param name="equipmentDescription">The equipment description.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchEquipment(string equipmentDescription)
        {
            var equipments = _equipmentService.SearchEquipment(equipmentDescription);
            var title = "Equipment Listing";
            if (!string.IsNullOrEmpty(equipmentDescription))
            {
                title = $"Search Result for {equipmentDescription}";
            }

            ViewBag.title = title;
            return View("SearchEquipment", equipments);
        }

        /// <summary>
        /// Downloads the invoice.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public FileStreamResult DownloadInvoice()
        {
            var invoice = _equipmentService.GenerateInvoice();

            return File(invoice, "text/plain", "CustomerInvoice.txt");
        }
    }
}