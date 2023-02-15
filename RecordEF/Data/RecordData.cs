using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using RecordEF.Data;
using RecordEF.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecordEF.Data
{
    public class RecordData
    {
        /// <summary>
        /// Insert a new record.
        /// </summary>
        public static Record CreateRecord(Record record)
        {
            using (var context = new RecordDbContext())
            {
                context.Records.Add(record);
                context.SaveChanges();

                var newRecord = context.Records.FirstOrDefault(r => r.Name == record.Name);
                return newRecord ?? new Record { RecordId = 0 };
            }
        }

        /// <summary>
        /// Update a record using variables.
        /// </summary>
        public static Record UpdateRecord(Record record)
        {
            using (var context = new RecordDbContext())
            {
                var recordToUpdate = context.Records.FirstOrDefault(r => r.RecordId == record.RecordId);

                if (recordToUpdate != null)
                {
                    // I have to add the ArtistId into the new record.
                    var artistId = recordToUpdate.ArtistId;
                    record.ArtistId = artistId;
                    context.Entry(recordToUpdate).CurrentValues.SetValues(record);
                    context.SaveChanges();
                }

                var updatedRecord = context.Records.FirstOrDefault(r => r.RecordId == record.RecordId);
                return updatedRecord ?? new Record { RecordId = 0 };
            }
        }

        /// <summary>
        /// Delete record.
        /// </summary>
        public static int DeleteRecord(int recordId)
        {
            var recId = 0;

            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);
                if (record != null)
                {
                    recId = record.RecordId;
                    context.Records.Remove(record);
                    context.SaveChanges();
                }
            }

            return recId;
        }

        public static List<Record> GetRecords()
        {
            var list = new List<Record>();
            using (var context = new RecordDbContext())
            {
                return list = context.Records.OrderBy(r => r.ArtistId).ThenBy(r => r.Recorded).ToList();
            }
        }

        public static dynamic? GetArtistRecordEntity(int recordId)
        {
            using (var context = new RecordDbContext())
            {
                var record = context.Records
                    .Join(context.Artists, r => r.ArtistId, a => a.ArtistId, (r, a) => new
                    {
                        ArtistId = a.ArtistId,
                        Artist = a.Name,
                        RecordId = r.RecordId,
                        Name = r.Name,
                        Recorded = r.Recorded,
                        Rating = r.Rating,
                        Media = r.Media,
                        Review = r.Review
                    })
                    .FirstOrDefault(r => r.RecordId == recordId);

                if (record != null)
                {
                    return record;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Artist and their record details.
        /// Called by GetRecordList3
        /// </summary>
        public static IEnumerable<dynamic> GetRecordList()
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .OrderBy(r => r.artist.LastName)
                    .ThenBy(r => r.artist.FirstName)
                    .ThenBy(r => r.record.Recorded)
                    .ToList();

                return records.Select(r => new
                {
                    ArtistId = r.artist.ArtistId,
                    Artist = r.artist.Name,
                    RecordId = r.record.RecordId,
                    Name = r.record.Name,
                    Recorded = r.record.Recorded,
                    Rating = r.record.Rating,
                    Media = r.record.Media
                });
            }
        }

        public static IEnumerable<dynamic> GetArtistRecordByYear(int year)
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.record.Recorded == year)
                    .OrderBy(r => r.artist.LastName)
                    .ThenBy(r => r.artist.FirstName)
                    .ToList();

                return records.Select(r => new
                {
                    ArtistId = r.artist.ArtistId,
                    Artist = r.artist.Name,
                    RecordId = r.record.RecordId,
                    Name = r.record.Name,
                    Recorded = r.record.Recorded,
                    Rating = r.record.Rating,
                    Media = r.record.Media
                });
            }
        }

        /// <summary>
        /// Get record details including the artist name.
        /// </summary>
        public static string GetArtistRecord(int recordId)
        {
            var artistRecord = new StringBuilder();

            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);

                if (record is Record)
                {
                    var artist = context.Artists.FirstOrDefault(r => r.ArtistId == record.ArtistId);

                    if (artist is Artist)
                    {
                        artistRecord.Append($"{artist.Name}");
                    }

                    artistRecord.Append($" {record.ToString()}");
                }
                else
                {
                    artistRecord.Append($"Record with Id: {recordId} not found!");
                }
            }

            return artistRecord.ToString();
        }

        /// <summary>
        /// Get record details.
        /// </summary>
        //public static Record GetRecordEntity(int recordId)
        //{
        //    using (var context = new RecordDbContext())
        //    {
        //        var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);

        //        if (record is Record)
        //        {
        //            return record;
        //        }
        //        else
        //        {
        //            Record missingRecord = new()
        //            {
        //                RecordId = 0
        //            };

        //            return missingRecord;
        //        }
        //    }
        //}

        public static Record GetRecordEntity(int recordId)
        {
            using (var context = new RecordDbContext())
            {
                var record = context.Records.FirstOrDefault(r => r.RecordId == recordId);
                return record ?? new Record { RecordId = 0 };
            }
        }

        /// <summary>
        /// Count the number of discs.
        /// </summary>
        public static int CountAllDiscs(string media = "")
        {
            StringBuilder count = new StringBuilder();
            var mediaTypes = new List<string>();

            switch (media)
            {
                case "":
                    mediaTypes = new List<string> { "DVD", "CD/DVD", "Blu-ray", "CD/Blu-ray", "CD", "R" };
                    break;
                case "DVD":
                    mediaTypes = new List<string> { "DVD", "CD/DVD", "Blu-ray", "CD/Blu-ray" };
                    break;
                case "CD":
                    mediaTypes = new List<string> { "CD" };
                    break;
                case "R":
                    mediaTypes = new List<string> { "R" };
                    break;
                default:
                    break;
            }

            using (var context = new RecordDbContext())
            {
                var sumOfDiscs = context.Records
                    .Where(r => mediaTypes.Contains(r.Media))
                    .Sum(r => r.Discs);

                return (int)sumOfDiscs;
            }
        }

        /// <summary>
        /// Get number of records for an artist.
        /// </summary>
        public static int GetArtistNumberOfRecords(int artistId)
        {
            using (var context = new RecordDbContext())
            {
                var sumOfDiscs = context.Records
                    .Where(r => r.ArtistId == artistId)
                    .Select(r => r.Discs)
                    .Sum();

                return sumOfDiscs > 0 ? (int)sumOfDiscs : 0;
            }
        }

        /// <summary>
        /// Get record details from ToString method.
        /// </summary>
        public static string GetFormattedRecord(int recordId)
        {
            using (var context = new RecordDbContext())
            {
                var record = context.Records.Find(recordId);

                return record != null ? record.ToString() : string.Empty; ;
            }
        }

        public static string GetArtistNameFromRecord(int recordId)
        {
            var artistName = string.Empty;
            using (var context = new RecordDbContext())
            {
                var record = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { record, artist })
                    .Where(r => r.record.RecordId == recordId)
                    .ToList();

                if (record.Any())
                {
                    artistName = record[0]?.artist?.Name ?? string.Empty;
                }
            }

            return artistName;
        }

        /// <summary>
        /// Get the number of discs for a particular year.
        /// </summary>
        public static int GetDiscCountForYear(int year)
        {
            var discCount = 0;
            using (var context = new RecordDbContext())
            {
                var records = context.Records.ToList();
                var numberOfDiscs = records
                .Where(r => r.Recorded == year)
                .Select(r => r.Discs)
                .Sum();
                discCount = (int)numberOfDiscs;
            }
            return discCount;
        }

        /// <summary>
        /// Get the number of discs that I bought for a particular year.
        /// </summary>
        public static int GetBoughtDiscCountForYear(int year)
        {
            var discCount = 0;
            using (var context = new RecordDbContext())
            {
                var records = context.Records.ToList();
                var numberOfDiscs = records
                .Where(r => r.Bought != null && r.Bought.Value.Year == year)
                .Select(r => r.Discs)
                .Sum();
                discCount = (int)numberOfDiscs;
            }
            return discCount;
        }

        /// <summary>
        /// Get a list of records without reviews.
        /// </summary>
        public static IEnumerable<dynamic> MissingRecordReviews()
        {
            using (var context = new RecordDbContext())
            {
                var records = context.Records
                    .Join(context.Artists, record => record.ArtistId, artist => artist.ArtistId, (record, artist) => new { artist, record })
                    .Where(r => string.IsNullOrEmpty(r.record.Review))
                    .OrderBy(r => r.artist.LastName).ThenBy(r => r.artist.FirstName)
                    .ToList();

                return records.Select(r => new
                {
                    ArtistId = r.artist.ArtistId,
                    Artist = r.artist.Name,
                    RecordId = r.record.RecordId,
                    Name = r.record.Name,
                    Recorded = r.record.Recorded,
                    Rating = r.record.Rating,
                    Media = r.record.Media
                });
            }
        }

        public static int GetMissingReviewsCount()
        {
            using (var context = new RecordDbContext())
            {
                return context.Records.Count(r => string.IsNullOrEmpty(r.Review));
            }
        }

        /// <summary>
        /// Get total number of discs for each artist.
        /// </summary>
        public static IEnumerable<dynamic> GetTotalArtistDiscs()
        {
            var list = new List<dynamic>();

            using (var context = new RecordDbContext())
            {
                return context.Artists
                          .OrderBy(a => a.LastName)
                          .ThenBy(a => a.FirstName)
                          .Select(artist => new
                          {
                              ArtistName = artist.Name,
                              Discs = context.Records.Where(r => r.ArtistId == artist.ArtistId).Sum(r => r.Discs)
                          })
                          .ToList();
            }
        }

        /// <summary>
        /// Get total cost spent on each artist.
        /// </summary>
        public static IEnumerable<dynamic> GetCostTotals()
        {
            var list = new List<dynamic>();

            using (var context = new RecordDbContext())
            {
                return context.Artists
                          .OrderBy(a => a.LastName)
                          .ThenBy(a => a.FirstName)
                          .Select(artist => new
                          {
                              ArtistName = artist.Name,
                              Cost = context.Records.Where(r => r.ArtistId == artist.ArtistId).Sum(r => r.Cost)
                          })
                          .ToList();
            }
        }
    }
}
