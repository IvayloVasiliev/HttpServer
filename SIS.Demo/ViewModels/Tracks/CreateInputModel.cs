using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Tracks
{
    public class CreateInputModel
    {
        public string AlbumId { get; set; }

        [StringLengthSis(3, 20, "ErrorMessage")]
        public string Name { get; set; }
        public string Link { get; set; }
        public decimal Price { get; set; }

    }
}
