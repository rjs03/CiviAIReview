using CiviAIReview.Models;
using Microsoft.AspNetCore.Mvc;
using OnShelfGTDL.Models;

namespace CiviAIReview.Controllers
{
    public class UsersController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public UsersController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserAccount()
        {
            return View();
        }


        [HttpPost]
        [Route("User/SaveUserInformation")]
        public async Task<IActionResult> SaveUserInformation([FromBody] UserModel model)
        {
            if (ModelState.IsValid)
            {
                // Call SaveUser method from DatabaseHelper
                bool isSaved = await _dbHelper.SaveUserAsync(
                    model.UserId,
                    model.MemberType,
                    model.FirstName,
                    model.MiddleName,
                    model.LastName,
                    model.Suffix,
                    model.Address,
                    model.EmailAddress,
                    model.MobileNumber,
                    model.Status
                );

                if (isSaved)
                {
                    return Json(new { success = true, message = "User saved successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save user." });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }

        [HttpGet]
        [Route("User/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dbHelper.GetUsersAsync();  // Ensure your DatabaseHelper has this method

            return Json(users);
        }

        [HttpPost]
        [Route("User/UpdateUserInformation")]
        public async Task<IActionResult> UpdateUserInformation([FromBody] UserModel model)
        {
            if (ModelState.IsValid)
            {
                // Call SaveUser method from DatabaseHelper
                bool isSaved = await _dbHelper.UpdateUserAsync(
                    model.UserId,
                    model.MemberType,
                    model.FirstName,
                    model.MiddleName,
                    model.LastName,
                    model.Suffix,
                    model.Address,
                    model.EmailAddress,
                    model.MobileNumber,
                    model.Status
                );

                if (isSaved)
                {
                    return Json(new { success = true, message = "User saved successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save user." });
                }
            }

            return Json(new { success = false, message = "Invalid data provided." });
        }

    }
}
