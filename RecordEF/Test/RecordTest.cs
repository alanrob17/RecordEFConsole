using RecordEF.Data;
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
    internal class RecordTest
    {
        internal static void CreateRecord(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                if (!context.Artists.Any(a => a.ArtistId == artistId))
                {
                    Console.WriteLine("ERROR: artist with specified ID does not exist!");
                    return;
                }
            }

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

            var message = rec.RecordId > 0 ? $"New record created with Id: {rec.RecordId}" : "ERROR: record not created!";
            Console.WriteLine(message);
        }

        internal static void UpdateRecord(int recordId)
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

        internal static void DeleteRecord(int recordId)
        {
            var recId = _rd.DeleteRecord(recordId);
            if (recId != 0)
            {
                Console.WriteLine($"Record with Id: {recId} deleted.");
            }
        }

        internal static void RecordHtml(int recordId)
        {
            var r = _rd.GetArtistRecordEntity(recordId);

            if (r != null)
            {
                Console.WriteLine($"<p><strong>ArtistId:</strong> {r.ArtistId}</p>\n<p><strong>Artist:</strong> {r.Artist}</p>\n<p><strong>RecordId:</strong> {r.RecordId}</p>\n<p><strong>Recorded:</strong> {r.Recorded}</p>\n<p><strong>Name:</strong> {r.Name}</p>\n<p><strong>Rating:</strong> {r.Rating}</p>\n<p><strong>Media:</strong> {r.Media}\n\n<p><strong>Review:</strong></p>\n<div>{r.Review}</div>\n");
            }
        }

        internal static void GetTotalArtistDiscs()
        {
            var list = _rd.GetTotalArtistDiscs();

            foreach (var item in list)
            {
                Console.WriteLine($"Total number of discs for {item.ArtistName} is {item.Discs}.");
            }
        }

        internal static void GetTotalArtistCost()
        {
            var list = _rd.GetCostTotals();

            foreach (var item in list)
            {
                Console.WriteLine($"Total cost for {item.ArtistName} is ${item.Cost:F2}.");
            }
        }

        internal static void GetNoReviewCount()
        {
            var count = _rd.GetMissingReviewsCount();

            Console.WriteLine($"The total number of missing reviews is {count}");
        }

        internal static void GetNoRecordReview()
        {
            var records = _rd.MissingRecordReviews();

            foreach (var record in records)
            {
                Console.WriteLine($"{record.Artist} - Id: {record.RecordId} - {record.Name} - {record.Recorded}");
            }
        }

        internal static void GetBoughtDiscCountForYear(int year)
        {
            var count = _rd.GetBoughtDiscCountForYear(year);

            Console.WriteLine($"The total number of discs bought in {year} is {count}.");
        }

        internal static void GetDiscCountForYear(int year)
        {
            var count = _rd.GetDiscCountForYear(year);

            Console.WriteLine($"The total number of discs for {year} are {count}.");
        }

        internal static void GetArtistNameFromRecord(int recordId)
        {
            var name = _rd.GetArtistNameFromRecord(recordId);
            Console.WriteLine(name);
        }

        internal static void GetRecordDetails(int recordId)
        {
            var record = _rd.GetFormattedRecord(recordId);

            Console.WriteLine(record);
        }

        internal static void GetArtistNumberOfRecords(int artistId)
        {
            var artist = _ad.GetArtistName(artistId);
            var recordNumber = _rd.GetArtistNumberOfRecords(artistId);
            Console.WriteLine($"{artist} has {recordNumber} discs.");
        }

        internal static void GetArtistRecordEntity(int recordId)
        {
            var r = _rd.GetArtistRecordEntity(recordId);

            if (r != null)
            {
                Console.WriteLine($"{r.Artist}\n");
                Console.WriteLine($"\t{r.Recorded} - {r.Name} ({r.Media}) - Rating: {r.Rating}");
            }
        }

        internal static void CountDiscs(string media)
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

        internal static void GetRecordEntity(int recordId)
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

        internal static void GetRecordById(int recordId)
        {
            var artistRecord = _rd.GetArtistRecord(recordId);

            Console.WriteLine(artistRecord);
        }

        // Artist - Record list - better code.
        internal static void GetRecordList()
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
        internal static void GetRecordList2()
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
        internal static void GetRecordList3()
        {
            var list = _rd.GetRecordList();

            foreach (var record in list)
            {
                Console.WriteLine($"{record.Artist}: {record.Name} - {record.Recorded} - {record.Media}");
            }
        }

        internal static void GetArtistRecordByYear(int year)
        {
            var list = _rd.GetArtistRecordByYear(year);
            Console.WriteLine($"List of records for {year}.");

            foreach (var record in list)
            {
                Console.WriteLine($"{record.Artist}: {record.Name} - {record.Recorded} - {record.Media}");
            }
        }

        internal static void GetArtistByRecordId(int recordId)
        {
            var artist = _rd.GetArtistByRecordId(recordId);
            if (artist.ArtistId > 0)
            {
                Console.WriteLine(artist.ToString());
            }
            else
            {
                Console.WriteLine("Artist not found!");
            }
        }

        internal static void GetArtistRecords(int artistId)
        {
            var records = _rd.GetArtistRecords(artistId);

            foreach (var record in records)
            {
                Console.WriteLine(record.ToString());
            }
        }

        //internal static void GetArtistRecordByYear2(int year)
        //{
        //    var list = _rd.GetArtistRecordByYear2(year);
        //    Console.WriteLine($"List of records for {year}.");

        //    foreach (var record in list)
        //    {
        //        Console.WriteLine($"{record.Artist}: {record.Name} - {record.Recorded} - {record.Media}");
        //    }
        //}
    }
}
