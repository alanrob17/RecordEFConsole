using RecordEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _ad = RecordEF.Data.ArtistData;
using _rd = RecordEF.Data.RecordData;

namespace RecordEF.Test
{
    internal class ArtistTest
    {
        internal static void GetArtistEntity(int artistId)
        {
            var artist = _ad.GetArtistEntity(artistId);

            if (artist.ArtistId > 0)
            {
                Console.WriteLine(artist.ToString());
            }
        }

        internal static void GetBiography(int artistid)
        {
            var biography = _ad.GetBiography(artistid);

            if (biography.Length > 5)
            {
                Console.WriteLine(biography);
            }
        }

        internal static void ArtistHtml(int artistId)
        {
            var artist = _ad.ShowArtist(artistId);

            Console.WriteLine(artist);
        }

        internal static void GetArtistId(string firstName, string lastName)
        {
            int artistId = _ad.GetArtistId(firstName, lastName);

            if (artistId > 0)
            {
                Console.WriteLine($"Artist Id is {artistId}");
            }
            else
            {
                Console.WriteLine("ERROR: Artist name not found.");
            }
        }

        internal static void GetArtistId(int recordId)
        {
            int artistId = _ad.GetArtistId(recordId);

            if (artistId > 0)
            {
                Console.WriteLine($"Artist Id is {artistId}");
            }
            else
            {
                Console.WriteLine("ERROR: Artist name not found.");
            }
        }

        internal static void DeleteArtist(int artistId)
        {
            var success = _ad.DeleteArtist(artistId);

            if (success)
            {
                Console.WriteLine($"Artist with Id: {artistId} deleted.");
            }
            else
            {
                Console.WriteLine($"ERROR: Couldn't delete Artist with Id: {artistId}!");
            }
        }

        internal static void UpdateArtist(int artistId)
        {
            Artist artist = new()
            {
                ArtistId = artistId,
                FirstName = "Joseph",
                LastName = "Whopposoni",
                Name = "",
                Biography = "Joe is an Italian country and western singer. He likes both kinds of music."
            };

            bool success = _ad.UpdateArtist(artist);

            if (success)
            {
                Console.WriteLine($"Successfully update artist with Id: {artistId}");
            }
            else
            {
                Console.WriteLine("ERROR: Artist couldn't be updated!");
            }
        }

        internal static void CreateArtist()
        {
            Artist artist = new()
            {
                FirstName = "Alan",
                LastName = "Robson",
                Name = "",
                Biography = "Alan is a country and western singer. He likes both kinds of music."
            };

            artist.Name = string.IsNullOrEmpty(artist.FirstName) ? artist.LastName : $"{artist.FirstName} {artist.LastName}";

            var exists = _ad.CheckForArtistName(artist.Name);

            if (exists)
            {
                Console.WriteLine("Artist Already exists in the Database!");
            }
            else
            {
                var id = _ad.InsertArtist(artist);

                if (id > 0)
                {
                    Console.WriteLine($"Artist created with Id: {id}");
                }
                else
                {
                    Console.WriteLine("New artist record not created!");
                }
            }
        }

        internal static void GetArtistNames()
        {
            var artists = _ad.GetArtists();
            foreach (var artist in artists)
            {
                Console.WriteLine($"{artist.ArtistId} - {artist.Name}");
            }
        }

        /// <summary>
        /// Get Artist details.
        /// </summary>
        internal static void GetArtist(int artistId)
        {
            var artist = _ad.GetArtist(artistId);

            if (artist.ArtistId > 0)
            {
                Console.WriteLine($"Artist Id: {artist.ArtistId}.\nName: {artist?.Name},\n Biography: {artist?.Biography}");
            }
            else
            {
                Console.WriteLine($"Artist with Id: {artistId} not found!");
            }
        }

        /// <summary>
        /// Get Artist name.
        /// </summary>
        internal static void GetArtistName(int artistId)
        {
            var artist = _ad.GetArtistName(artistId);

            if (!string.IsNullOrEmpty(artist))
            {
                Console.WriteLine(artist);
            }
            else
            {
                Console.WriteLine($"Artist with Id: {artistId} not found!");
            }
        }

        /// <summary>
        /// Select all artists with no biography.
        /// </summary>
        internal static void GetArtistsWithNoBio()
        {
            var artists = _ad.GetArtistsWithNoBio();

            if (artists.Any())
            {
                foreach (var artist in artists)
                {
                    Console.WriteLine($"Id: {artist.ArtistId} - {artist.Name}");
                }
            }
        }

        /// <summary>
        /// Get the count of Artist records with no Biography.
        /// </summary>
        internal static void GetNoBiographyCount()
        {
            var count = _ad.NoBiographyCount();

            if (count > 0)
            {
                Console.WriteLine($"The number of records with no artist biography is: {count}.");
            }
        }

        internal static void GetArtistByName(string artistName)
        {

            var artist = _ad.GetArtistByName(artistName);

            Console.WriteLine(artist);
        }

        internal static void GetArtistById(int artistId)
        {
            var artistRecords = _ad.GetArtistById(artistId);

            Console.WriteLine(artistRecords);
        }
    }
}
