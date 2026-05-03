using Civil_Registration_System_Platform.Repositories.Implementations;
using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class GovernorateServices : IGovernorateServices
    {
        private readonly IGovernorateRepository _governorateRepository;
        public GovernorateServices(IGovernorateRepository governorateRepository)
        {
            _governorateRepository = governorateRepository;
        }
        public async Task<List<GovernorateGetAll>> GetAllGovernoratesAsync()
        {
            var governorates  =await _governorateRepository.GetActiveGovernoratesAsync();
            List<GovernorateGetAll> governorateGetAlls = new List<GovernorateGetAll>();
            foreach (var governorate in governorates)
            {
                governorateGetAlls.Add(new GovernorateGetAll
                {
                    GovernorateId = governorate.GovernorateId,
                    Name = governorate.Name,
                    Code = governorate.Code
                });
            }
            return governorateGetAlls;
        }
    }
}
