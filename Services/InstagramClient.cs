using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstaSharp;
using InstaSharp.Endpoints;
using InstaSharp.Models;
using InstaSharp.Models.Responses;
using Ninject;

namespace JuniperBot.Services {

    internal class InstagramClient : AbstractService {

        [Inject]
        public ConfigurationManager ConfigurationManager
        {
            get; set;
        }

        private InstagramConfig Config;

        private OAuthResponse AuthInfo;

        private Users UsersEndPoint;

        private List<InstaSharp.Models.Media> Cache = new List<InstaSharp.Models.Media>();

        private long LatestUpdate
        {
            get;
            set;
        }

        protected override void Init() {
            Config = new InstagramConfig(
                ConfigurationManager.Config.Instagram.ClientId,
                ConfigurationManager.Config.Instagram.ClientSecret,
                ConfigurationManager.Config.Instagram.RedirectUri);
            AuthInfo = new OAuthResponse() {
                AccessToken = ConfigurationManager.Config.Instagram.Token,
                User = new UserInfo() {
                    FullName = "Test",
                    Username = "Test"
                }
            };
            UsersEndPoint = new Users(Config, AuthInfo);
        }

        public async Task<List<InstaSharp.Models.Media>> GetRecent() {
            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (currentTimestamp > LatestUpdate + ConfigurationManager.Config.Instagram.TTL) {
                var userFeed = await UsersEndPoint.Recent(ConfigurationManager.Config.Instagram.UserId);
                lock (this) {
                    if (userFeed.Meta.Code == System.Net.HttpStatusCode.OK) {
                        Cache = userFeed.Data;
                    }
                }
            }
            return Cache;
        }
    }
}