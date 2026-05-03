using Civil_Registration_System_Platform.Account.AccountViewModel;
using Microsoft.AspNetCore.Identity;


using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;



namespace Civil_Registration_System_Platform.Account.AccountServices
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IWebHostEnvironment _env;

        public AccountServices(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager , IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        public async Task<string> RegisterUserAsync(RegisterViewModel registerViewModel)
        {
            UserAccount userAccount = new UserAccount
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                FullName = registerViewModel.FullName,
                EGPhoneNumber = registerViewModel.EGPhoneNumber,
                NationalID = registerViewModel.NationalID,
                Gender = registerViewModel.Gender,
                MaritalStatus = registerViewModel.MaritalStatus,
                GovernorateId = registerViewModel.GovernorateId,
                OfficeId = registerViewModel.OfficeId,
                IsConfirmed = false 
            };
            await _userManager.AddToRoleAsync(userAccount, "User");
            userAccount = await CreateUserImage(registerViewModel.CardImage, userAccount);

            IdentityResult result = await _userManager.CreateAsync(userAccount, registerViewModel.PassWord);

            if (result.Succeeded)
            {
                return "User Registered Successfully";
            }
            else
            {
                string errors = "User Registration Failed: ";
                foreach (var ErrorItem in result.Errors)
                {
                    errors += ErrorItem.Description + " , ";
                }
                return errors;
            }
        }

        public async Task<string> RegisterEmployeeAsync(RegisterAdminOrEmployeeViewModel registerEmployeeViewModel)
        {
            UserAccount userAccount = new UserAccount
            {
                UserName = registerEmployeeViewModel.UserName,
                Email = registerEmployeeViewModel.Email,
                FullName = registerEmployeeViewModel.FullName,
                EGPhoneNumber = registerEmployeeViewModel.EGPhoneNumber,
                NationalID = registerEmployeeViewModel.NationalID,
                Gender = registerEmployeeViewModel.Gender,
                MaritalStatus = registerEmployeeViewModel.MaritalStatus,
                GovernorateId = registerEmployeeViewModel.GovernorateId,
                OfficeId = registerEmployeeViewModel.OfficeId,
                ManageOfficeId = registerEmployeeViewModel.ManagerOfficeId,
                IsConfirmed = true  
            };
            await _userManager.AddToRoleAsync(userAccount, "Employee");
            userAccount = await CreateUserImage(registerEmployeeViewModel.CardImage, userAccount);
            IdentityResult result = await _userManager.CreateAsync(userAccount, registerEmployeeViewModel.PassWord);

            if (result.Succeeded)
            {
                return "Employee Registered Successfully";
            }
            else
            {
                string errors = "Employee Registration Failed: ";
                foreach (var ErrorItem in result.Errors)
                {
                    errors += ErrorItem.Description + " , ";
                }
                return errors;
            }
        }


        public async Task<string> RegisterAdminAsync(RegisterAdminOrEmployeeViewModel registerAdminViewModel)
        {
            UserAccount userAccount = new UserAccount
            {
                UserName = registerAdminViewModel.UserName,
                Email = registerAdminViewModel.Email,
                FullName = registerAdminViewModel.FullName,
                EGPhoneNumber = registerAdminViewModel.EGPhoneNumber,
                NationalID = registerAdminViewModel.NationalID,
                Gender = registerAdminViewModel.Gender,
                MaritalStatus = registerAdminViewModel.MaritalStatus,
                GovernorateId = registerAdminViewModel.GovernorateId,
                OfficeId = registerAdminViewModel.OfficeId,
                ManageOfficeId = registerAdminViewModel.ManagerOfficeId,
                IsConfirmed = true
            };
            await _userManager.AddToRoleAsync(userAccount, "Admin");
            userAccount = await CreateUserImage(registerAdminViewModel.CardImage, userAccount);
            IdentityResult result = await _userManager.CreateAsync(userAccount, registerAdminViewModel.PassWord);

            if (result.Succeeded)
            {
                return "Admin Registered Successfully";
            }
            else
            {
                string errors = "Admin Registration Failed: ";
                foreach (var ErrorItem in result.Errors)
                {
                    errors += ErrorItem.Description + " , ";
                }
                return errors;
            }
        }

        public async Task<string> LoginUserAsync(LoginViewModel loginViewModel)
        {           
            UserAccount? userAccount = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (userAccount != null)
            {
                bool found = await _userManager.CheckPasswordAsync(userAccount, loginViewModel.PassWord);
                if (found)
                {
                    await _signInManager.SignInAsync(userAccount, true);
                    return "Login Successful";  
                }
            else
                return "username or password worng";
            }
            return "username or password worng";
        }





        // creating image 

        
        public async Task<UserAccount> CreateUserImage(IFormFile cardImg, UserAccount userAccount)
        {
            string ext = Checking(cardImg);
            string fileName = await Create(cardImg, ext);
            userAccount.CardImagePath = $"/AccountCardImages/{fileName}";
            return userAccount;
        }
        


        private string Checking(IFormFile img)
        {

            if (img == null || img.Length == 0)
                throw new Exception("No image uploaded");

            // 2️⃣ تحقق من الامتداد
            var allowedExt = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(img.FileName).ToLower();
            if (!allowedExt.Contains(ext))
                throw new Exception("Invalid image type");

            //    // 1️⃣ تحقق من الحجم (1MB)
            if (img.Length > 1024 * 1024 * 5)
                throw new Exception("Max image size is 5MB");


            return ext;
        }


        private async Task<string> Create(IFormFile img, string ext)
        {
            // 3️⃣ GUID
            var guid = Guid.NewGuid().ToString("N");

            string folderPath = Path.Combine(_env.WebRootPath, "AccountCardImages");
            Directory.CreateDirectory(folderPath);


            // 5️⃣ المسار الكامل
            var fileName = guid + ext;
            var fullPath = Path.Combine(folderPath, fileName);


            // 6️⃣ حفظ الصورة + ضغط
            using var imageSharp = Image.Load(img.OpenReadStream());
            imageSharp.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(1024, 1024)
            }));
            // ضغط JPEG
            var encoder = new JpegEncoder { Quality = 80 };
            await imageSharp.SaveAsync(fullPath, encoder);

            return fileName;
        }


    }
}
