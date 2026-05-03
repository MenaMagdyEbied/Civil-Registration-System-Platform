using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class OfficeServices : IOfficeServices
    {
        private readonly IOfficeRepository _officeRepository;
        public OfficeServices(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }
        public async Task<IEnumerable<OfficeGovernrate>> GetByGovernorateIdAsync(int governorateId)
        {
           var offices = await _officeRepository.GetByGovernorateIdAsync(governorateId);
           List<OfficeGovernrate> officeList = new List<OfficeGovernrate>();
           foreach (var office in offices)
           {
                officeList.Add(new OfficeGovernrate
                {
                    OfficeId = office.OfficeId,
                    Name = office.Name,
                    Phone = office.Phone
                });
           }
           return officeList;
        }

    }
}
