using PetsWebsite.Models.Repository;

namespace PetsWebsite.Logic
{
    public class CommonLogic : IcommonLogic
    {
        private readonly RestaurantsRepository restaurantsRepository;

        public CommonLogic(RestaurantsRepository restaurantsRepository)
        {
            this.restaurantsRepository = restaurantsRepository;
        }
        public List<string> GetExistCity()
        {
            return restaurantsRepository.GetCity();
        }

        public List<string> GetExistegion()
        {
            return restaurantsRepository.GetRegion();
        }
    }
}
