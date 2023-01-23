using RecordEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordEF.Data
{
    public class RecordData
    {
        /// <summary>
        /// Delete record.
        /// </summary>
        public static void DeleteRecord(int recordId)
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
        public static void GetRecord(int recordId)
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
        public static void UpdateRecord()
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
        public static void InsertRecord()
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
        public static void CountDiscs()
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
        public static void GetArtistNumberOfRecords(int artistId)
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
        public static void GetFormattedRecord(int recordId)
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
        public static void GetRecordedYearDiscCount(int year)
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
        public static void NoRecordReviews()
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
        public static void GetTotals()
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

        public static void GetArtistsAndRecords2()
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
        public static void GetArtistsAndRecords()
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
        public static void GetArtistRecordByYear2(int year)
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

        public static void GetArtistRecordByYear(int year)
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

    }
}
