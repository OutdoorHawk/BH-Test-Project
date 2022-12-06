namespace MirrorServiceTest.Code.Infrastructure.DI
{
    public class DIContainer 
    {
        private static DIContainer _instance;
        public static DIContainer Container => _instance ?? (_instance = new DIContainer());

        public void BindSingle<TService>(TService implementation) where TService : IService =>
            Implementation<TService>.ServiceInstance = implementation;

        public TService Resolve<TService>() where TService : IService =>
            Implementation<TService>.ServiceInstance;

        private class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}
