using Microsoft.EntityFrameworkCore;
using RecordEF.Data;
using RecordEF.Models;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace RecordEF
{
    public class Program
    {
        public static void Main(string[] args)
        {


            // GetArtists();
            // GetArtistById(114);
            // GetArtistByName("Bob Dylan");
            // GetArtistRecordByYear(1974);
            // GetArtistRecordByYear2(1974);
            // GetArtistsAndRecords();
            // GetArtistsAndRecords2();
            // GetTotals();
            // NoRecordReviews();
            // GetRecordedYearDiscCount(1974);
            // GetFormattedRecord(212);
            // GetArtistNumberOfRecords(114);
            // CountDiscs();
            // InsertRecord();
            // UpdateRecord();
            // UpdateRecord();
            // GetRecord(1135);
            // DeleteRecord(5252);
            // GetBiography(114);
            // ShowArtist(114);
            // InsertArtist();
            // UpdateArtist(828);
            // GetArtistId(string.Empty, "Led Zeppelin");
            DeleteArtist(828);
        }

        /// <summary>
        /// Delete an artist.
        /// </summary>
        private static void DeleteArtist(int artistId)
        {
            var artistData = new ArtistData();
            artistData.Delete(artistId);

            Console.WriteLine("Artist deleted");
        }

        /// <summary>
        /// Get artist id by firstName, lastName.
        /// </summary>
        private static void GetArtistId(string firstName, string lastName)
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
            private static void UpdateArtist(int artistId)
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
        private static void ShowArtist(int artistId)
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
        private static string ToHtml(Artist artist)
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
        private static void GetBiography(int artistId)
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

        /// <summary>
        /// Delete record.
        /// </summary>
        private static void DeleteRecord(int recordId)
        {
            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);
                if (record != null)
                {
                    context.Records.Remove(record);
                    context.SaveChanges();

                    Console.WriteLine($"Record with Id: {recordId} deleted.");
                }
            }

        }

        /// <summary>
        /// Get record details including the artist biography.
        /// </summary>
        private static void GetRecord(int recordId)
        {
            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);

                if (record is Record)
                {
                    var artist = context.Artists.FirstOrDefault(r => r.ArtistId == record.ArtistId);

                    if (artist is Artist)
                    {
                        Console.WriteLine($"{artist.Name}");
                    }

                    Console.WriteLine($"\t{record}");
                }
                else 
                { 
                    Console.WriteLine($"Record with Id: {recordId} not found!"); 
                }
            }
        }

        /// <summary>
        /// Update a record using variables.
        /// </summary>
        private static void UpdateRecord()
        {
            IFormatProvider culture = System.Threading.Thread.CurrentThread.CurrentCulture;

            var date = "21-06-2015";
            var recordId = 5251;

            using (var context = new RecordDbContext())
            {
                var recordToUpdate = context.Records.FirstOrDefault(r => r.RecordId == recordId);

                if (recordToUpdate != null)
                {
                    recordToUpdate.ArtistId = 528;
                    recordToUpdate.Name = "No Fun In Paradise";
                    recordToUpdate.Recorded = 1987;
                    recordToUpdate.Label = "Whoppo";
                    recordToUpdate.Pressing = "Au";
                    recordToUpdate.Field = "Jazz";
                    recordToUpdate.Rating = "****";
                    recordToUpdate.Discs = 1;
                    recordToUpdate.Media = "CD";
                    recordToUpdate.Bought = DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    recordToUpdate.Cost = 12.99m;
                    recordToUpdate.CoverName = string.Empty;
                    recordToUpdate.Review = "This is Alan's third album. Just before he went mad!";

                    context.SaveChanges();

                    Console.WriteLine($"Record Id: {recordId} updated.");
                }
            }
        }

        /// <summary>
        /// Insert a new record.
        /// </summary>
        private static void InsertRecord()
        {
            Record record = new()
            {
                ArtistId = 528,
                Name = "Fun In Paradise",
                Recorded = 1986,
                Label = "Whoppo",
                Pressing = "Au",
                Field = "Rock",
                Rating = "****",
                Discs = 1,
                Media = "CD",
                Bought = new DateTime(2015, 05, 06),
                Cost = 10.99m,
                CoverName = string.Empty,
                Review = "This is Alan's second album."
            };

            using (var context = new RecordDbContext())
            {
                var records = context.Records;
                records.Add(record);
                context.SaveChanges();

                var newRecord = context.Records.OrderByDescending(x => x.RecordId).FirstOrDefault();

                if (newRecord != null)
                {
                    Console.WriteLine($"The new record name is {newRecord.Name} and has a RecordId of: {newRecord.RecordId}");
                }
            }
        }

        /// <summary>
        /// Count the number of discs.
        /// </summary>
        private static void CountDiscs()
        {
            RecordDbContext context = new();

            var mediaTypes = new List<string> { "DVD", "CD/DVD", "Blu-ray", "CD/Blu-ray" };
            var sumOfDiscs = context.Records
                .Where(r => mediaTypes.Contains(r.Media))
                .Sum(r => r.Discs);

            if (sumOfDiscs > 0)
            {
                Console.WriteLine($"\tNumber of DVD or Blu-ray Discs: {sumOfDiscs}");
            }

            sumOfDiscs = context.Records
                .Where(r => r.Media == "CD")
                .Sum(r => r.Discs);

            if (sumOfDiscs > 0)
            {
                Console.WriteLine($"\tNumber of CD audio Discs: {sumOfDiscs}");
            }
        }

        /// <summary>
        /// Get number of records for an artist.
        /// </summary>
        private static void GetArtistNumberOfRecords(int artistId)
        {
            RecordDbContext context = new();

            var artist = context.Artists.FirstOrDefault(a => a.ArtistId == artistId);

            var sumOfDiscs = context.Records
                .Where(r => r.ArtistId == 114)
                .Select(r => r.Discs)
                .Sum();

            if (sumOfDiscs > 0 && artist is Artist)
            {
                Console.WriteLine($"{artist.Name}");
                Console.WriteLine($"\tNumber of Discs: {sumOfDiscs}");
            }
        }

        /// <summary>
        /// Get record details from ToString method.
        /// </summary>
        private static void GetFormattedRecord(int recordId)
        {
            RecordDbContext context = new();

            var record = context.Records.SingleOrDefault(r => r.RecordId == recordId);

            if (record is Record)
            {
                Console.WriteLine(record);
            }
        }

        /// <summary>
        /// Get the number of discs for a particular year.
        /// </summary>
        private static void GetRecordedYearDiscCount(int year)
        {
            RecordDbContext context = new();

            var records = context.Records.ToList();

            var numberOfDiscs = records
                .Where(r => r.Recorded == year)
                .Select(r => r.Discs)
                .Sum();

            Console.WriteLine($"The total number of discs for Year: {year} are {numberOfDiscs}");
        }

        /// <summary>
        /// Get a list of records without reviews.
        /// </summary>
        private static void NoRecordReviews()
        {
            RecordDbContext context = new();

            var artists = context.Artists.OrderBy(a => a.LastName).ThenBy((a => a.FirstName)).ToList();
            var records = context.Records.ToList();

            foreach (var r in records)
            {
                Console.WriteLine($"{r.Artist.Name} - Id: {r.RecordId} - {r.Name} - {r.RecordId}");
            }
        }

        /// <summary>
        /// Get total cost spent on each artist.
        /// </summary>
        private static void GetTotals()
        {
            RecordDbContext context = new();

            var artists = context.Artists.OrderBy(a => a.LastName).ThenBy((a => a.FirstName)).ToList();
            var records = context.Records.ToList();

            foreach (var a in artists)
            {
                var cost = a.Records.Sum(c => c.Cost);
                var discs = a.Records.Sum(d => d.Discs);

                Console.WriteLine($"{a.Name}\n\t Total discs: {discs} - Total cost: ${cost:F2}\n");
            }

        }

        private static void GetArtistsAndRecords2()
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .OrderBy(r => r.artist.LastName)
                    .ThenBy(r => r.artist.FirstName)
                    .ThenByDescending(r => r.record.Recorded)
                    .ToList();

                foreach (var r in records)
                {
                    Console.WriteLine($"{r.artist.Name}: {r.record.Name} - {r.record.Recorded} - {r.record.Media}");
                }
            }
        }

        // better output than version 2
        private static void GetArtistsAndRecords()
        {
            RecordDbContext context = new();

            var artists = context.Artists.OrderBy(a => a.LastName).ThenBy((a => a.FirstName)).ToList();
            var records = context.Records.ToList();

            foreach (var a in artists)
            {
                Console.WriteLine($"{a.Name}");


                var collection = a.Records.OrderByDescending(r => r.Recorded).ThenBy(r => r.Media).ToList();

                foreach (var r in collection)
                {
                    Console.WriteLine($"\t{r.Recorded} - {r.Name} - ({r.Media})");
                }

                Console.WriteLine();
            }
        }

        // another version of this method
        private static void GetArtistRecordByYear2(int year)
        {
            RecordDbContext context = new();

            var artists = context.Artists.OrderBy(a => a.LastName).ThenBy((a => a.FirstName)).ToList();

            var records = context.Records.Where(r => r.Recorded == year).ToList();

            var results = artists
                .Join(records, artist => artist.ArtistId, record => record.ArtistId, (artist, record) => new { artist, record })
                .Where(x => x.record.Recorded == year)
                .OrderBy(x => x.artist.LastName)
                .ThenBy(x => x.artist.FirstName)
                .ThenBy(x => x.record.Name)
                .ThenBy(x => x.record.Media)
                .ToList();

            Console.WriteLine($"Recordings by year: {year}");

            foreach (var r in results)
            {
                Console.WriteLine($"\t{r.artist.Name} - {r.record.Name} - {r.record.Recorded} ({r.record.Media})");
            }
        }

        private static void GetArtistRecordByYear(int year)
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.record.Recorded == year)
                    .OrderBy(r => r.artist.LastName)
                    .ThenBy(r => r.artist.FirstName)
                    .ToList();

                if (records.Any())
                {
                    Console.WriteLine($"Recordings by year: {year}");

                    foreach (var r in records)
                    {
                        Console.WriteLine($"\t{r.artist.Name} - {r.record.Name} - {r.record.Recorded} ({r.record.Media})");
                    }
                }
            }
        }

        private static void GetArtistByName(string artistName)
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.artist.Name.Contains(artistName))
                    .OrderBy(r => r.record.Recorded)
                    .ToList();

                Console.WriteLine($"{artistName}:");

                foreach (var r in records)
                {
                    Console.WriteLine($"\t{r.record.Name} - {r.record.Recorded} - {r.record.Media}");
                }
            }
        }

        private static void GetArtistById(int id)
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

        private static void GetArtists()
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
