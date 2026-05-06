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
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IWebHostEnvironment _env;

        public AccountServices(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager , IUserAccountRepository userAccountRepository, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userAccountRepository = userAccountRepository; 
            _env = env;
        }

        public async Task<string> RegisterUserAsync(RegisterViewModel registerViewModel)
        {
            if (await _userManager.FindByNameAsync(registerViewModel.UserName) != null)
                return "اسم المستخدم مستخدم بالفعل. اختر اسم مستخدم آخر.";

            if (await _userManager.FindByEmailAsync(registerViewModel.Email) != null)
                return "البريد الإلكتروني مستخدم بالفعل.";

            if (await _userManager.Users.AnyAsync(u => u.NationalID == registerViewModel.NationalID))
                return "الرقم القومي مستخدم بالفعل.";

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
            userAccount = await CreateUserImage(registerViewModel.CardImage, userAccount);

            IdentityResult result = await _userManager.CreateAsync(userAccount, registerViewModel.PassWord);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userAccount, "User");
                return "User Registered Successfully";
            }
            else
            {
                string errors = "فشل تسجيل المستخدم: ";
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
                ManageOfficeId = null,
                IsConfirmed = true  
            };
            userAccount = await CreateUserImage(registerEmployeeViewModel.CardImage, userAccount);
            IdentityResult result = await _userManager.CreateAsync(userAccount, registerEmployeeViewModel.PassWord);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userAccount, "Employee");
                return "Employee Registered Successfully";
            }
            else
            {
                string errors = "فشل تسجيل الموظف: ";
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
                OfficeId = null,
                ManageOfficeId = null,
                IsConfirmed = true
            };
            
            userAccount = await CreateUserImage(registerAdminViewModel.CardImage, userAccount);
            IdentityResult result = await _userManager.CreateAsync(userAccount, registerAdminViewModel.PassWord);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userAccount, "Admin");
                return "Admin Registered Successfully";
            }
            else
            {
                string errors = "فشل تسجيل الأدمن: ";
                foreach (var ErrorItem in result.Errors)
                {
                    errors += ErrorItem.Description + " , ";
                }
                return errors;
            }
        }

        public async Task<string> LoginUserAsync(LoginViewModel loginViewModel)
        {
       
            UserAccount? userAccount =
                   await _userManager.FindByNameAsync(loginViewModel.UserName)
                ?? await _userManager.FindByEmailAsync(loginViewModel.UserName)
                ?? await _userManager.Users.FirstOrDefaultAsync(u => u.NationalID == loginViewModel.UserName);

            if (userAccount == null)
                return "username or password worng";

            bool passwordOk = await _userManager.CheckPasswordAsync(userAccount, loginViewModel.PassWord);
            if (!passwordOk)
                return "username or password worng";

            if (userAccount.IsRejected)
                return $"تم رفض حسابك: {userAccount.RejectionMessage ?? "تواصل مع الإدارة"}";

            await _signInManager.SignInAsync(userAccount, true);
            return "Login Successful";
        }



        public async Task<GetMyAccount> GetMyAccount()
        {
            UserAccount userAccount = await _userAccountRepository.GetMyAccount();

            GetMyAccount getMyAccount = new GetMyAccount
            {
                UserId = userAccount.Id,
                FullName = userAccount.FullName,
                EGPhoneNumber = userAccount.EGPhoneNumber,
                NationalID = userAccount.NationalID,
                Email = userAccount.Email,
                CardImagePath = userAccount.CardImagePath,
                Gender = userAccount.Gender,
                MaritalStatus = userAccount.MaritalStatus,
                IsConfirmed = userAccount.IsConfirmed,
                CreatedAt = userAccount.CreatedAt,
                IsRejected = userAccount.IsRejected,
                RejectionMessage = userAccount.RejectionMessage,
                GovernorateName = userAccount.Governorate.Name,
                OfficeName = userAccount.Office.Name
            };

            return getMyAccount;
        }




        public async Task<string> EditMyAccount(UserAccountEdit userAccountEdit)
        {
            UserAccount userAccount = await _userAccountRepository.GetMyAccount();

            userAccount.FullName = userAccountEdit.FullName;    
            userAccount.EGPhoneNumber = userAccountEdit.EGPhoneNumber;
            userAccount.NationalID = userAccountEdit.NationalID;
            userAccount.Gender = userAccountEdit.Gender;
            userAccount.MaritalStatus = userAccountEdit.MaritalStatus;
            userAccount.GovernorateId = userAccountEdit.GovernorateId;
            userAccount.OfficeId = userAccountEdit.OfficeId;

            await _userManager.SetEmailAsync(userAccount, userAccountEdit.Email);

            // deleting old image   
            try
            {
                await DeleteImg(userAccount);
            }
            catch (Exception ex)
            {
                throw new Exception( $"Error deleting old image: {ex.Message}");
            }
            userAccount = await CreateUserImage(userAccountEdit.CardImage, userAccount);


            userAccount.IsRejected = false; 
            userAccount.RejectionMessage = null;

            await _userAccountRepository.SaveUser(userAccount); 
            return "تم تحديث الحساب بنجاح";
        }








        private async Task<bool> DeleteImg(UserAccount user)
        {
            if (user.CardImagePath == null)
                return false;
             
            string fullPath = Path.Combine(
                _env.WebRootPath,
                "AccountCardImages",
                Path.GetFileName(user.CardImagePath)
            );

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            user.CardImagePath = null;

            await _userAccountRepository.SaveUser(user);
            return true;
        }


        // creating image 


        public async Task<UserAccount> CreateUserImage(IFormFile? cardImg, UserAccount userAccount)
        {
            if (cardImg == null || cardImg.Length == 0)
            {
                userAccount.CardImagePath = userAccount.CardImagePath ?? string.Empty;
                return userAccount;
            }

            string ext = Checking(cardImg);
            string fileName = await Create(cardImg, ext);
            userAccount.CardImagePath = $"/AccountCardImages/{fileName}";
            return userAccount;
        }
        


        private string Checking(IFormFile img)
        {

            if (img == null || img.Length == 0)
                throw new Exception("لم يتم رفع صورة");

            // 2️⃣ تحقق من الامتداد
            var allowedExt = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(img.FileName).ToLower();
            if (!allowedExt.Contains(ext))
                throw new Exception("نوع الصورة غير مسموح");

            //    // 1️⃣ تحقق من الحجم (1MB)
            if (img.Length > 1024 * 1024 * 5)
                throw new Exception("الحد الأقصى لحجم الصورة هو 5 ميجابايت");


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
