namespace JuniperBot.Model.Exception {

    public class DiscordException : System.Exception {

        public DiscordException() {
        }

        public DiscordException(string message) : base(message) {
        }

        public DiscordException(string message, System.Exception innerException) : base(message, innerException) {
        }
    }
}