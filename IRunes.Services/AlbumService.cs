using IRunes.Data;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.Services
{
    public class AlbumService : IAlbumService
    {
        private RunesDbContext context;

        public AlbumService(RunesDbContext runesDbContext)
        {
            this.context = runesDbContext;
        }

        public bool AddTrackToAlbum(string albumId, Track trackFromDb)
        {
            Album albumFromDb = this.GetAlbumById(albumId);

            if (albumFromDb == null)
            {
                return false;
            }

            albumFromDb.Tracks.Add(trackFromDb);
            albumFromDb.Price = (albumFromDb.Tracks
                                    .Select(track => track.Price)
                                    .Sum() * 87) / 100;

            this.context.Update(albumFromDb);
            this.context.SaveChanges();
            return true;
        }

        public Album CreateAlbum(Album album)
        {
            album = context.Albums.Add(album).Entity;
            context.SaveChanges();

            return album;
        }

        public Album GetAlbumById(string id)
        {
            return context.Albums
                .Include(album => album.Tracks)
                .SingleOrDefault(album => album.Id == id);
        }

        public ICollection<Album> GetAllAlbums()
        {
            return context.Albums.ToList();

        }
    }
}
