using Civil_Registration_System_Platform.Repositories.Interfaces;
using Civil_Registration_System_Platform.Services.Interfaces;
using System.Collections.Generic;

namespace Civil_Registration_System_Platform.Services.Implementations
{
    public class ServicesApplicationsServices : IServicesApplicationsServices
    {
        private readonly IServicesApplicationsReopsitory _servicesApplicationsReopsitory;

        public ServicesApplicationsServices(IServicesApplicationsReopsitory servicesApplicationsReopsitory)
        {
            _servicesApplicationsReopsitory = servicesApplicationsReopsitory;   
        }

        public async Task<List<ServicesApplicationsVM>> GetAllServicesWithApplications()
        {
            List<ServicesApplicationsVM> servicesApplicationsVMs = new List<ServicesApplicationsVM>();
            List<ServicesTypeHelper> servicesTypeHelper = await _servicesApplicationsReopsitory.GetAll();

            Dictionary<ServiceType, List<AppDetail>> keyValuePairs = new Dictionary<ServiceType, List<AppDetail>>();
            foreach (ServicesTypeHelper serviceTypeHelper in servicesTypeHelper)
            {
                if (!keyValuePairs.ContainsKey((ServiceType)serviceTypeHelper.ServicesTypeEnum))
                {
                    keyValuePairs[(ServiceType)serviceTypeHelper.ServicesTypeEnum] = new List<AppDetail>();
                }

                keyValuePairs[(ServiceType)serviceTypeHelper.ServicesTypeEnum].Add(new AppDetail
                {
                    applicationType = (ApplicationType)serviceTypeHelper.ApplicationTypeEnum,
                    Min = serviceTypeHelper.MinDays,
                    Max = serviceTypeHelper.MaxDays,
                    Price = serviceTypeHelper.Price,
                    Details = serviceTypeHelper.Details
                });
            }


            foreach(var keyValuePair in keyValuePairs)
            {
                servicesApplicationsVMs.Add(new ServicesApplicationsVM
                {
                    serviceType = keyValuePair.Key,
                    appDetails = keyValuePair.Value
                });
            }

            return servicesApplicationsVMs;
        }



        public async Task<string> UpdateServiceApplication(ServicesApplicationsAddOrUpdateVM servicesApplicationsAddOrUpdateVM)
        {

            ServicesTypeHelper servicesTypeHelper = await _servicesApplicationsReopsitory
                 .GetByIds(servicesApplicationsAddOrUpdateVM.ServicesTypeEnum, servicesApplicationsAddOrUpdateVM.ApplicationTypeEnum);
            if (servicesTypeHelper == null){
                throw new Exception("not found service type with application");
            }

            servicesTypeHelper.Price = servicesApplicationsAddOrUpdateVM.Price;
            servicesTypeHelper.MinDays = servicesApplicationsAddOrUpdateVM.min;
            servicesTypeHelper.MaxDays = servicesApplicationsAddOrUpdateVM.max;
            servicesTypeHelper.Details = servicesApplicationsAddOrUpdateVM.Details;

            await _servicesApplicationsReopsitory.Update(servicesTypeHelper);
            return "updated success";
        }
    }
}
