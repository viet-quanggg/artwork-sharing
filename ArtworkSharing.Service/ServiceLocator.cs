using ArtworkSharing.Core.Interfaces.Services;

namespace ArtworkSharing.Service
{
    public static class ServiceLocator
    {
        private static IFireBaseService _firebaseService;
        private static IWatermarkService _watermarkService;

        public static void Initialize(IFireBaseService firebaseService, IWatermarkService watermarkService)
        {
            _firebaseService = firebaseService;
            _watermarkService = watermarkService;
        }

        public static IFireBaseService GetFirebaseService()
        {
            if (_firebaseService == null)
            {
                throw new InvalidOperationException("IFirebaseService has not been initialized.");
            }

            return _firebaseService;
        }
        public static IWatermarkService GetWatermarkService()
        {
            if (_watermarkService == null)
            {
                throw new InvalidOperationException("IFirebaseService has not been initialized.");
            }

            return _watermarkService;
        }
    }
}
