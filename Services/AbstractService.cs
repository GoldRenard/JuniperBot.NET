namespace JuniperBot.Services {

    internal abstract class AbstractService : IService {

        private bool IsInitialized
        {
            get; set;
        }

        public void Initialize() {
            if (!IsInitialized) {
                Init();
                IsInitialized = true;
            }
        }

        protected abstract void Init();
    }
}