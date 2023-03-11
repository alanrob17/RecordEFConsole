using RecordEF.Data;
using RecordEF.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordEF.Data
{
    public class ArtistData
    {
        public static List<Artist> GetArtists()
        {
            var list = new List<Artist>();
            using (var db = new RecordDbContext())
            {
                return list = db.Artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList();
            }
        }

        public static string GetArtistName(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
                return artist?.Name ?? string.Empty;
            }
        }

        public static Artist GetArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                return context.Artists.FirstOrDefault(a => a.ArtistId == artistId) ?? new Artist { ArtistId = 0 };
            }
        }

        public static List<Artist> GetArtistsWithNoBio()
        {
            using (var context = new RecordDbContext())
            {
                var artists = context.Artists.Where(a => string.IsNullOrEmpty(a.Biography))
                    .OrderBy(a => a.LastName)
                    .ThenBy(a => a.FirstName)
                    .ToList();

                return artists;
            }
        }

        public static int NoBiographyCount()
        {
            var number = 0;
            using (var context = new RecordDbContext())
            {
                return number = context.Artists.Where(a => string.IsNullOrEmpty(a.Biography)).Count();
            }
        }

        /// <summary>
        /// Check if an Artist already exists.
        /// </summary>
        public static bool CheckForArtistName(string name)
        {
            using (var context = new RecordDbContext())
            {
                return context.Artists.Any(a => a.Name == name);
            }
        }

        /// <summary>
        /// Insert a new artist entity.
        /// </summary>
        public static int InsertArtist(Artist artist)
        {
            using (var context = new RecordDbContext())
            {
                context.Artists.Add(artist);
                context.SaveChanges();

                var newArtist = context.Artists.OrderByDescending(x => x.ArtistId).FirstOrDefault();

                return newArtist?.ArtistId ?? 0;
            }
        }

        /// <summary>
        /// Update an artist.
        /// </summary>
        public static bool UpdateArtist(Artist artist)
        {
            using (var context = new RecordDbContext())
            {
                var artistToUpdate = context.Artists.FirstOrDefault(a => a.ArtistId == artist.ArtistId);
                if (artistToUpdate == null) return false;

                artistToUpdate.FirstName = artist.FirstName;
                artistToUpdate.LastName = artist.LastName;
                artistToUpdate.Name = string.IsNullOrEmpty(artist.FirstName) ? artist.LastName : $"{artist.FirstName} {artist.LastName}";
                artistToUpdate.Biography = artist.Biography;

                context.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Delete an artist.
        /// </summary>
        public static bool DeleteArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.Find(artistId);

                if (artist != null)
                {
                    context.Artists.Remove(artist);
                    context.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Get artist id by firstName, lastName.
        /// </summary>
        public static int GetArtistId(string firstName, string lastName)
        {
            var name = string.IsNullOrEmpty(firstName) ? lastName : $"{firstName} {lastName}";

            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase));

                return artist?.ArtistId ?? 0;
            }
        }

        internal static int GetArtistId(int recordId)
        {
            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);

                return record?.ArtistId ?? 0;
            }
        }

        /// <summary>
        /// Show an artist as Html.
        /// </summary>
        public static string ShowArtist(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

                if (artist != null)
                {
                    return ToHtml(artist);
                }
            }

            return string.Empty;
        }


        /// <summary>
        /// ToHtml method shows an instances properties
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <returns>The <see cref="string"/> artist record as a string.</returns>
        public static string ToHtml(Artist artist)
        {
            var artistDetails = new StringBuilder();

            artistDetails.Append($"<p><strong>ArtistId: </strong>{artist.ArtistId}</p>\n");

            if (!string.IsNullOrEmpty(artist.FirstName))
            {
                artistDetails.Append($"<p><strong>First Name: </strong>{artist.FirstName}</p>\n");
            }

            artistDetails.Append($"<p><strong>Last Name: </strong>{artist.LastName}</p>\n");

            if (!string.IsNullOrEmpty(artist.Name))
            {
                artistDetails.Append($"<p><strong>Name: </strong>{artist.Name}</p>\n");
            }

            if (!string.IsNullOrEmpty(artist.Biography))
            {
                artistDetails.Append($"<p><strong>Biography: </strong></p>\n<div>\n{artist.Biography}\n</div>\n");
            }

            return artistDetails.ToString();
        }

        /// <summary>
        /// Get biography from the current record Id.
        /// </summary>
        public static string GetBiography(int artistId)
        {
            var bio = new StringBuilder();

            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
                if (artist != null)
                {
                    bio.Append($"Name: {artist.Name}\n");
                    bio.Append($"Biography:\n{artist.Biography}");
                }
            }

            return bio.ToString();
        }

        public static string GetArtistByName(string artistName)
        {
            var artistRecords = new StringBuilder();

            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.artist.Name != null && r.artist.Name.ToLower().Contains(artistName.ToLower()))
                    .OrderBy(r => r.record.Recorded)
                    .ToList();

                artistRecords.Append($"{artistName}\n");

                if (records.Any())
                {
                    foreach (var r in records)
                    {
                        artistRecords.Append($"\t{r.record.Name} - {r.record.Recorded} - {r.record.Media}\n");
                    }
                }
            }

            return artistRecords.ToString();
        }

        public static string GetArtistById(int artistId)
        {
            var artistRecords = new StringBuilder();

            using (var context = new RecordDbContext())
            {
                var records = context.Records
                                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                                    .Where(r => r.artist.ArtistId == artistId)
                                    .OrderBy(r => r.record.Recorded)
                                    .ToList();

                if (records.Any())
                {
                    artistRecords.Append($"{records[0].artist.ArtistId} - {records[0].artist.Name}\n\n");

                    foreach (var r in records)
                    {
                        artistRecords.Append($"\t{r.record.Name} - {r.record.Recorded} ({r.record.Media})\n");
                    }
                }
            }

            return artistRecords.ToString();
        }

        public static Artist GetArtistEntity(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
                return artist ?? new Artist { ArtistId = 0 };
            }
        }

        /// <summary>
        /// Fill the artist drop down list.
        /// </summary>
        /// <returns>The <see cref="Dictionary"/>Artist list.</returns>
        internal static Dictionary<int, string> GetArtistDropDownList()
        {
            var artistDictionary = new Dictionary<int, string>();

            using (var context = new RecordDbContext())
            {
                var artists = context.Artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName);

                artistDictionary.Add(0, "Select an artist");

                foreach (var artist in artists)
                {
                    if (string.IsNullOrEmpty(artist.FirstName))
                    {
                        artistDictionary.Add(artist.ArtistId, artist.LastName);
                    }
                    else
                    {
                        artistDictionary.Add(artist.ArtistId, $"{artist.LastName}, {artist.FirstName}");
                    }
                }
            }

            return artistDictionary;
        }

        /// <summary>
        /// Check if an Artist already exists.
        /// </summary>
        internal static string GetArtistNameByRecordId(int recordId)
        {
            Artist? artist = new();

            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(a => a.RecordId == recordId);
                if (record != null)
                {
                    artist = context.Artists.SingleOrDefault(a => a.ArtistId == record.ArtistId);
                }

                if (artist != null)
                {
                    return artist.Name ?? "Unknown artist";
                }

                return string.Empty;
            }
        }
    }
}
