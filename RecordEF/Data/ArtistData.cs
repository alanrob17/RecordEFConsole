using RecordEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordEF.Data
{
    public class ArtistData
    {
        /// <summary>
        /// Show a list of artist Names.
        /// </summary>
        public static void GetArtistNames()
        {
            using (var context = new RecordDbContext())
            {
                var artists = context.Artists.OrderBy(a => a.LastName).ThenBy((a => a.FirstName)).ToList();

                foreach (Artist a in artists)
                {
                    Console.WriteLine(a.Name);
                }
            }
        }

        /// <summary>
        /// Get Artist details including the artist biography.
        /// </summary>
        public static void GetArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                if (artist is Artist)
                {
                    Console.WriteLine(artist);
                }
                else
                {
                    Console.WriteLine($"Artist with Id: {artistId} not found!");
                }
            }
        }

        /// <summary>
        /// Select all artists with no biography.
        /// </summary>
        public static void SelectArtistWithNoBio()
        {
            using (var context = new RecordDbContext())
            {
                var artists = context.Artists.Where(a => string.IsNullOrEmpty(a.Biography))
                    .OrderBy(a => a.LastName)
                    .ThenBy(a => a.FirstName)
                    .ToList();

                if (artists.Any())
                {
                    foreach (var artist in artists)
                    {
                        Console.WriteLine($"{artist}");
                    }
                }
            }
        }

        /// <summary>
        /// Delete an artist.
        /// </summary>
        public static void DeleteArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(r => r.ArtistId == artistId);

                if (artist != null)
                {
                    context.Artists.Remove(artist);
                    context.SaveChanges();

                    Console.WriteLine($"Artist with Id: {artistId} deleted.");
                }
            }
        }

        /// <summary>
        /// Get artist id by firstName, lastName.
        /// </summary>
        public static void GetArtistId(string firstName, string lastName)
        {
            var name = string.IsNullOrEmpty(firstName) ? lastName : $"{firstName} {lastName}";

            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.Name == name);

                if (artist != null)
                {
                    Console.WriteLine($"Artist Id is {artist.ArtistId}");
                }
            }
        }

        /// <summary>
        /// Update an artist.
        /// </summary>
        public static void UpdateArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artistToUpdate = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                if (artistToUpdate != null)
                {
                    artistToUpdate.FirstName = "Alan";
                    artistToUpdate.LastName = "Robsano";
                    artistToUpdate.Name = string.IsNullOrEmpty(artistToUpdate.FirstName) ? artistToUpdate.LastName : $"{artistToUpdate.FirstName} {artistToUpdate.LastName}";
                    artistToUpdate.Biography = "Alan hates country and western. He hates both kinds of music.";

                    context.SaveChanges();

                    Console.WriteLine($"Artist Id: {artistToUpdate.ArtistId} updated.");
                }
            }
        }

        /// <summary>
        /// Insert a new artist entity.
        /// </summary>
        public static void InsertArtist()
        {
            Artist artist = new()
            {
                FirstName = "Alano",
                LastName = "Robosono",
                Name = "",
                Biography = "Alan is a country and western singer. He likes both kinds of music."
            };

            artist.Name = string.IsNullOrEmpty(artist.FirstName) ? artist.LastName : $"{artist.FirstName} {artist.LastName}";

            using (var context = new RecordDbContext())
            {
                var artists = context.Artists;
                artists.Add(artist);
                context.SaveChanges();

                var newArtist = context.Artists.OrderByDescending(x => x.ArtistId).FirstOrDefault();

                if (newArtist != null)
                {
                    Console.WriteLine($"{newArtist}");
                }
            }
        }

        /// <summary>
        /// Show an artist as Html.
        /// </summary>
        public static void ShowArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                if (artist != null)
                {
                    string artistRecord = ToHtml(artist);

                    Console.WriteLine(artistRecord);
                }
            }
        }

        /// <summary>
        /// ToHtml method shows an instances properties
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <returns>The <see cref="string"/> artist record as a string.</returns>
        public static string ToHtml(Artist artist)
        {
            var artistDetails = new StringBuilder();

            artistDetails.Append($"<strong>ArtistId: </strong>{artist.ArtistId}<br/>\n");

            if (!string.IsNullOrEmpty(artist.FirstName))
            {
                artistDetails.Append($"<strong>First Name: </strong>{artist.FirstName}<br/>\n");
            }

            artistDetails.Append($"<strong>Last Name: </strong>{artist.LastName}<br/>\n");

            if (!string.IsNullOrEmpty(artist.Name))
            {
                artistDetails.Append($"<strong>Name: </strong>{artist.Name}<br/>\n");
            }

            if (!string.IsNullOrEmpty(artist.Biography))
            {
                artistDetails.Append($"<strong>Biography: </strong>{artist.Biography}<br/>\n");
            }

            return artistDetails.ToString();
        }

        /// <summary>
        /// Get biography from the current record Id.
        /// </summary>
        public static void GetBiography(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
                if (artist != null)
                {
                    Console.WriteLine($"Name: {artist.Name}");
                    Console.WriteLine($"Biography: {artist.Biography}");
                }
            }
        }

        public static void GetArtistByName(string artistName)
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.artist.Name != null && r.artist.Name.Contains(artistName))
                    .OrderBy(r => r.record.Recorded)
                    .ToList();

                Console.WriteLine($"{artistName}:");

                if (records.Any())
                {
                    foreach (var r in records)
                    {
                        Console.WriteLine($"\t{r.record.Name} - {r.record.Recorded} - {r.record.Media}");
                    }
                }
            }
        }

        public static void GetArtistById(int id)
        {
            RecordDbContext context = new();

            var artist = context.Artists.FirstOrDefault(a => a.ArtistId == id);
            var records = context.Records.Where(r => r.ArtistId == id).OrderByDescending(r => r.Recorded).ToList();

            if (artist is Artist)
            {
                Console.WriteLine($"{artist.Name} - {artist.ArtistId}");

                foreach (var r in records)
                {
                    Console.WriteLine($"\t{r.Name} - {r.Recorded} ({r.Media})");
                }
            }
        }

        public static void GetArtists()
        {
            RecordDbContext context = new();

            var artists = context.Artists.OrderBy(a => a.LastName).ThenBy((a => a.FirstName)).ToList();

            foreach (Artist a in artists)
            {
                Console.WriteLine($"{a.Name} - {a.ArtistId}");
            }
        }
    }
}
