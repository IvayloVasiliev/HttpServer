using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Users
{
    public class UserLoginInputModel
    {
        private const string ErrorMassege = "Invalid username or password";

        [RequiredSis(ErrorMassege)]
        public string Username { get; set; }

        [RequiredSis(ErrorMassege)]
        public string Password { get; set; }

    }
}
