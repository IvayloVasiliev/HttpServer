using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Albums
{
    public class AlbumCreateInputModel
    {
        private const string NameErrorMessage = "Inavalid Length! Name must be between" +
            " 3 and 30 symbols!";

        private const string CoverErrorMessage = "Inavalid Length! Cover must be between" +
            " 5 and 255 symbols!";

        [RequiredSis]
        [StringLengthSis(3, 30, NameErrorMessage)]
        public string Name { get; set; }

        [RequiredSis]
        [StringLengthSis(3, 255, CoverErrorMessage)]
        public string Cover { get; set; }
    }
}
