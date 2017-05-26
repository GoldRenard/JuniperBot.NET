using System;

namespace JuniperBot.Model.Exception {

    [Serializable]
    internal class ValidationException : DiscordException {

        public ValidationException() {
        }

        public ValidationException(string message) : base(message) {
        }

        public ValidationException(string message, System.Exception innerException) : base(message, innerException) {
        }
    }
}