using SIS.MvcFramework.Attributes.Validation;

namespace IRunes.App.ViewModels.Tracks
{
    public class TrackDetailsInputModel
    {
        [RequiredSis]
        public string AlbumId { get; set; }

        [RequiredSis]
        public string TrackId { get; set; }

    }
}
