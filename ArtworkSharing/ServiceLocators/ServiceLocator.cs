using ArtworkSharing.Core.Interfaces.Services;

namespace ArtworkSharing.ServiceLocators
{
    public static class ServiceLocator
    {
        private static IFireBaseService _firebaseService;

        public static void Initialize(IFireBaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public static IFireBaseService GetFirebaseService()
        {
            if (_firebaseService == null)
            {
                throw new InvalidOperationException("IFirebaseService has not been initialized.");
            }

            return _firebaseService;
        }
    }

}
