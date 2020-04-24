using IRunes.Data;
using IRunes.Models;
using System.Linq;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        private RunesDbContext context;

        public TrackService(RunesDbContext runesDbContext)
        {
            this.context = runesDbContext;
        }

   

        public Track GetTrackById(string trackId)
        {
            return this.context.Tracks
                .SingleOrDefault(track => track.Id == trackId);
        }
    }
}
