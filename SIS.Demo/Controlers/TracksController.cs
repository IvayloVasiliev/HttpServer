namespace IRunes.App.Controlers
{
    using IRunes.App.ViewModels;
    using IRunes.App.ViewModels.Tracks;
    using IRunes.Models;
    using IRunes.Services;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Results;

    public class TracksController : Controller
    {
        private readonly ITrackService trackService;
        private readonly IAlbumService albumService;

        public TracksController(ITrackService trackService, IAlbumService albumService)
        {
            this.trackService = trackService;
            this.albumService = albumService;
        }

        [Authorize]
        public ActionResult Create(string albumId)
        {
            return this.View(new TrackCreateViewModel { AlbumId = albumId});
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateInputModel model)
        {
            Track trackFromDb = new Track
            {
                Name = model.Name,
                Link = model.Link,
                Price = model.Price
            }; 

            if (!this.albumService.AddTrackToAlbum(model.AlbumId, trackFromDb))
            {
                return this.Redirect("/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={model.AlbumId}");
        }


        [Authorize]
        public ActionResult Details(string trackId, string albumId)
        {
            Track trackFromDb = this.trackService.GetTrackById(trackId);

            if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?id={albumId}");
            }

            TrackDetailsViewModel trackDetailsViewModel = ModelMapper.ProjectTo<TrackDetailsViewModel>(trackFromDb);
            trackDetailsViewModel.AlbumId = albumId;

            return this.View(trackDetailsViewModel);
        }

    }
}
