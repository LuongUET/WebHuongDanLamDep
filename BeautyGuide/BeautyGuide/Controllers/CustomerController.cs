using BeautyGuide.Helper;
using BeautyGuide.Models;
using BeautyGuide.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using BeautyGuide.Controllers;


namespace BeautyGuide.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index(string SearchString, string Status)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if(HttpContext.Session.GetString("SessionRoleId") == "2")
            {
                return RedirectToAction(nameof(CustomerInterfaceController.Index), "CustomerInterface");
            }

            UserViewModel userViewModel = new UserViewModel();
            userViewModel.UserDetailList = new List<UserDetail>();
            var dataUser = new UserQuery().GetAllUsers(SearchString, 2);
            foreach (var item in dataUser)
            {
                userViewModel.UserDetailList.Add(new UserDetail
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Avatar = item.Avatar,

                    LastLogin = item.LastLogin,
                    LastLogout = item.LastLogout,

                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
            return View(userViewModel);
        }
        [HttpGet]
        public IActionResult Add()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (HttpContext.Session.GetString("SessionRoleId") == "2")
            {
                return RedirectToAction(nameof(CustomerInterfaceController.Index), "CustomerInterface");
            }
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.UserDetailList = new List<UserDetail>();
            var dataRole = new UserQuery().GetRole();
            UserDetail model = new UserDetail();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserDetail user, IFormFile AvatarFile)
        {
            ModelState.Remove("AvatarFile");
            if (ModelState.IsValid)
            {
                // khong co loi tu phia nguoi dung
                // upload file va lay dc ten file save database
                string avatarNameFile = null;
                if (AvatarFile != null)
                {
                    avatarNameFile = await UploadFileHelper.UploadFile(AvatarFile);
                }

                try
                {
                    int idInsetCate = new UserQuery().InsertItemUser(user.RoleId, user.UserName, user.Password, user.Email, avatarNameFile);
                    if (idInsetCate > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                }
                catch (Exception ex)
                {

                    TempData["saveStatus"] = false;
                    return Ok(ex.Message);
                }
                return RedirectToAction(nameof(CustomerController.Index), "Customer");
            }


            UserViewModel userViewModel = new UserViewModel();
            userViewModel.UserDetailList = new List<UserDetail>();
            var dataRole = new UserQuery().GetRole();
            foreach (var item in dataRole)
            {
                userViewModel.UserDetailList.Add(new UserDetail
                {
                    RoleId = item.RoleId,
                    NameRole = item.NameRole

                });
            }
            IEnumerable<UserDetail> userDetails = userViewModel.UserDetailList;
            ViewBag.userViewModel = userDetails;

            return View(user);


        }
        [HttpGet]
        public IActionResult ViewDetail(int id)
        {
            UserDetail userDetail = new UserQuery().GetViewDetail(id);

            return View(userDetail);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (HttpContext.Session.GetString("SessionRoleId") == "2")
            {
                return RedirectToAction(nameof(CustomerInterfaceController.Index), "CustomerInterface");
            }
            UserDetail userDetail = new UserQuery().GetViewDetail(id);

            return View(userDetail);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserDetail userDetail, IFormFile AvatarFile)
        {
            try
            {
                var detail = new UserQuery().GetViewDetail(userDetail.Id);
                string avatar = detail.Avatar; // lay lai ten anh cu truoc khi thay anh moi (neu co)
                //nguoi dung co muon thay anh poster category hay ko?
                if (userDetail.AvatarFile != null)
                {
                    //co muon thay doi anh
                    avatar = await UploadFileHelper.UploadFile(AvatarFile);
                }
                bool update = new UserQuery().UpdateUserById(
                     userDetail.UserName, userDetail.Password, userDetail.Email, avatar, userDetail.Id);
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(CustomerController.Index), "Customer");
            }
            catch (Exception ex)
            {
                //return Ok(ex.Message);
                return View(userDetail);
                //return Ok(uniquePosterImage);
            }

        }
        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            if (HttpContext.Session.GetString("SessionRoleId") == "2")
            {
                return RedirectToAction(nameof(CustomerInterfaceController.Index), "CustomerInterface");
            }
            bool del = new UserQuery().DeleteItemUser(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(CustomerController.Index), "Customer");
        }

    }
}
