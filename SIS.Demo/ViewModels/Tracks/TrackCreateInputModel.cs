using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Tracks
{
    public class TrackCreateInputModel
    {
        private const string NameErrorMessage = "Track name must be between 3 and 20 symbols!";

        private const string LinkErrorMessage = "Link name must be between 3 and 20 symbols!";

        private const string PriceErrorMessage = "Invalid price!";

        [RequiredSis]
        public string AlbumId { get; set; }

        [RequiredSis]
        [StringLengthSis(3, 20, NameErrorMessage)]
        public string Name { get; set; }

        [RequiredSis]
        [StringLengthSis(3, 20, LinkErrorMessage)]
        public string Link { get; set; }

        [RangeSis(typeof(decimal), "0.00", "79228162514264337593543950335", PriceErrorMessage)]
        public decimal Price { get; set; }

    }
}
