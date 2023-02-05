using Microsoft.EntityFrameworkCore;
using RecordEF.Data;
using RecordEF.Models;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using _ad = RecordEF.Data.ArtistData;
using _rd = RecordEF.Data.RecordData;

namespace RecordEF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //// Artists
            // CreateArtist();
            // GetArtistById(114);
            // GetArtistEntity(114);
            // GetArtistByName("Bruce Cockburn");
            // GetBiography(114);
            // ArtistHtml(114);
            // GetArtistId("Bob", "Dylan");
            // UpdateArtist(823);
            // DeleteArtist(823);
            // GetArtist(63);
            // GetArtistName(114);
            // GetArtistNames();
            // GetArtistsWithNoBio();
            // GetNoBiographyCount();

            //// Records
            // CreateRecord(823);
            // UpdateRecord(5251);
            // DeleteRecord(5252);
            // GetRecordById(2196);
            // GetRecordEntity(2196);
            // GetArtistRecordByYear(1974);
            // GetRecordList();
            // GetRecordList2();
            // GetRecordList3();
            // CountDiscs(string.Empty);
            // CountDiscs("DVD");
            // CountDiscs("CD");
            // CountDiscs("R");
            // GetArtistRecordEntity(2196);
            // GetArtistNumberOfRecords(114);
            // GetRecordDetails(2196);
            // GetArtistNameFromRecord(2196);
            // GetDiscCountForYear(2022);
            // GetBoughtDiscCountForYear(2014);
            // GetNoRecordReview();
            // GetNoReviewCount();
            // GetTotalArtistCost();
            // GetTotalArtistDiscs();
            // RecordHtml(2196);
        }

        private static void DeleteRecord(int recordId)
        {
            var recId = _rd.DeleteRecord(recordId);
            if (recId != 0)
            {
                Console.WriteLine($"Record with Id: {recId} deleted.");
            }
        }

        private static void UpdateRecord(int recordId)
        {
            Record record = new()
            {
                RecordId = recordId,
                Name = "Far Way More Fun In Paradise",
                Recorded = 1999,
                Label = "Whoppo",
                Pressing = "Au",
                Field = "Blues",
                Rating = "****",
                Discs = 1,
                Media = "CD",
                Bought = Convert.ToDateTime("21-06-2022"),
                Cost = 12.99m,
                Review = "This is Alan's third album. Just before he went mad!"
            };

            Record newRecord = _rd.UpdateRecord(record);

            Console.WriteLine(newRecord.ToString());
        }

        private static void CreateRecord(int artistId)
        {
            Record record = new()
            {
                ArtistId = artistId,
                Name = "No Fun In Paradise",
                Recorded = 1988,
                Label = "Whoppo",
                Pressing = "Au",
                Field = "Rock",
                Rating = "***",
                Discs = 1,
                Media = "CD",
                Bought = Convert.ToDateTime("06/08/2017"),
                Cost = 10.99m,
                Review = "This is Alan's third album."
            };

            var rec = _rd.CreateRecord(record);

            if (rec.RecordId > 0)
            {
                Console.WriteLine($"New record created with Id: {rec.RecordId}");
            }
            else
            {
                Console.WriteLine("ERROR: record not created!");
            }
        }

        private static void RecordHtml(int recordId)
        {
            var r = _rd.GetArtistRecordEntity(recordId);

            if (r != null)
            {
                Console.WriteLine($"<p><strong>ArtistId:</strong> {r.ArtistId}</p>\n<p><strong>Artist:</strong> {r.Artist}</p>\n<p><strong>RecordId:</strong> {r.RecordId}</p>\n<p><strong>Recorded:</strong> {r.Recorded}</p>\n<p><strong>Name:</strong> {r.Name}</p>\n<p><strong>Rating:</strong> {r.Rating}</p>\n<p><strong>Media:</strong> {r.Media}\n");
            }
        }

        private static void GetTotalArtistDiscs()
        {
            var list = _rd.GetTotalArtistDiscs();

            foreach (var item in list)
            {
                Console.WriteLine($"Total number of discs for {item.ArtistName} is {item.Discs}.");
            }
        }

        private static void GetTotalArtistCost()
        {
            var list = _rd.GetCostTotals();

            foreach (var item in list)
            {
                Console.WriteLine($"Total cost for {item.ArtistName} is ${item.Cost:F2}.");
            }
        }

        private static void GetNoReviewCount()
        {
            var count = _rd.SumOfMissingReviews();

            Console.WriteLine($"The total number of missing reviews is {count}");
        }

        private static void GetNoRecordReview()
        {
            var records = _rd.MissingRecordReviews();

            foreach (var record in records)
            {
                Console.WriteLine($"{record.Artist} - Id: {record.RecordId} - {record.Name} - {record.Recorded}");
            }
        }

        private static void GetBoughtDiscCountForYear(int year)
        {
            var count = _rd.GetBoughtDiscCountForYear(year);

            Console.WriteLine($"The total number of discs bought in {year} is {count}.");
        }

        private static void GetDiscCountForYear(int year)
        {
            var count = _rd.GetDiscCountForYear(year);

            Console.WriteLine($"The total number of discs for {year} are {count}.");
        }

        private static void GetArtistNameFromRecord(int recordId)
        {
            var name = _rd.GetArtistNameFromRecord(recordId);
            Console.WriteLine(name);
        }

        private static void GetRecordDetails(int recordId)
        {
            var record = _rd.GetFormattedRecord(recordId);

            Console.WriteLine(record);
        }

        private static void GetArtistNumberOfRecords(int artistId)
        {
            var artist = _ad.GetArtistName(artistId);
            var recordNumber = _rd.GetArtistNumberOfRecords(artistId);
            Console.WriteLine($"{artist} has {recordNumber} discs.");
        }

        private static void GetArtistRecordEntity(int recordId)
        {
            var r = _rd.GetArtistRecordEntity(recordId);

            if (r != null)
            {
                Console.WriteLine($"{r.Artist}\n");
                Console.WriteLine($"\t{r.Recorded} - {r.Name} ({r.Media}) - Rating: {r.Rating}");
            }
        }

        private static void CountDiscs(string media)
        {
            var discs = _rd.CountAllDiscs(media);

            switch (media)
            {
                case "":
                    Console.WriteLine($"The total number of all discs is: {discs}");
                    break;
                case "DVD":
                    Console.WriteLine($"The total number of all DVD, CD/DVD Blu-ray or CD/Blu-ray discs is: {discs}");
                    break;
                case "CD":
                    Console.WriteLine($"The total number of audio discs is: {discs}");
                    break;
                case "R":
                    Console.WriteLine($"The total number of vinyl discs is: {discs}");
                    break;
                default:
                    break;
            }
        }

        private static void GetArtistEntity(int artistId)
        {
            var artist = _ad.GetArtistEntity(artistId);

            if (artist.ArtistId > 0)
            {
                Console.WriteLine(artist.ToString());
            }
        }

        private static void GetRecordEntity(int recordId)
        {
            var record = _rd.GetRecordEntity(recordId);

            if (record.RecordId > 0)
            {
                Console.WriteLine($"Id: {record.RecordId} - Name: {record.Name} - Recorded: {record.Recorded} - Media: {record.Media}");
            }
            else
            {
                Console.WriteLine($"Record with Id: {recordId} not found!");
            }
        }

        private static void GetRecordById(int recordId)
        {
            var artistRecord = _rd.GetArtistRecord(recordId);

            Console.WriteLine(artistRecord);
        }

        private static void GetBiography(int artistid)
        {
            var biography = _ad.GetBiography(artistid);

            if (biography.Length > 5)
            {
                Console.WriteLine(biography);
            }
        }

        private static void ArtistHtml(int artistId)
        {
            var artist = _ad.ShowArtist(artistId);

            Console.WriteLine(artist);
        }

        private static void GetArtistId(string firstName, string lastName)
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

        private static void DeleteArtist(int artistId)
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

        private static void UpdateArtist(int artistId)
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

        private static void CreateArtist()
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

        // Artist - Record list - better code.
        private static void GetRecordList()
        {
            var artistRecords = _ad.GetArtists().
                GroupJoin(
                    _rd.GetRecords(),
                    a => a.ArtistId,
                    r => r.ArtistId,
                    (a, r) => new { a, r }
                );

            foreach (var artist in artistRecords)
            {
                Console.WriteLine($"\n{artist.a.Name}\n");

                foreach (var record in artist.r)
                {
                    Console.WriteLine($"\t{record.Recorded} - {record.Name} ({record.Media})");
                }
            }
        }

        // Artist - Record list.
        private static void GetRecordList2()
        {
            var artists = _ad.GetArtists();
            var records = _rd.GetRecords();

            foreach (var artist in artists)
            {
                Console.WriteLine($"{artist.Name}:\n");

                var ar = from r in records
                         where artist.ArtistId == r.ArtistId
                         orderby r.Recorded descending
                         select r;

                foreach (var rec in ar)
                {
                    Console.WriteLine($"\t{rec.Recorded} - {rec.Name} ({rec.Media})");
                }

                Console.WriteLine();
            }
        }

        // All records in single row.
        private static void GetRecordList3()
        {
            var list = _rd.GetRecordList();

            foreach (var record in list)
            {
                Console.WriteLine($"{record.Artist}: {record.Name} - {record.Recorded} - {record.Media}");
            }
        }

        private static void GetArtistRecordByYear(int year)
        {
            var list = _rd.GetArtistRecordByYear(year);
            Console.WriteLine($"List of records for {year}.");

            foreach (var record in list)
            {
                Console.WriteLine($"{record.Artist}: {record.Name} - {record.Recorded} - {record.Media}");
            }
        }

        //private static void GetArtistRecordByYear2(int year)
        //{
        //    var list = _rd.GetArtistRecordByYear2(year);
        //    Console.WriteLine($"List of records for {year}.");

        //    foreach (var record in list)
        //    {
        //        Console.WriteLine($"{record.Artist}: {record.Name} - {record.Recorded} - {record.Media}");
        //    }
        //}

        private static void GetArtistNames()
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
        public static void GetArtist(int artistId)
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
        public static void GetArtistName(int artistId)
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
        public static void GetArtistsWithNoBio()
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
        private static void GetNoBiographyCount()
        {
            var count = _ad.NoBiographyCount();

            if (count > 0)
            {
                Console.WriteLine($"The number of records with no artist biography is: {count}.");
            }
        }

        public static void GetArtistByName(string artistName)
        {

            var artist = _ad.GetArtistByName(artistName);

            Console.WriteLine(artist);
        }

        private static void GetArtistById(int artistId)
        {
            var artistRecords = _ad.GetArtistById(artistId);

            Console.WriteLine(artistRecords);
        }
    }
}
