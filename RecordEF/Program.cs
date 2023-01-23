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
            /// Artist queries
            _ad.GetArtists();
            // _ad.GetArtistById(114);
            // _ad.GetArtistByName("Bob Dylan");
            // _ad.GetBiography(114);
            // _ad.ShowArtist(114);
            // _ad.InsertArtist();
            // _ad.UpdateArtist(828);
            // _ad.GetArtistId(string.Empty, "Led Zeppelin");
            // _ad.DeleteArtist(828);
            // _ad.SelectArtistWithNoBio();
            // _ad.GetArtist(114);
            // _ad.DeleteArtist(828);
            // _ad.GetArtistNames();

            /// Record queries
            // _rd.GetArtistRecordByYear(1974);
            // _rd.GetArtistRecordByYear2(1974);
            // _rd.GetArtistsAndRecords();
            // _rd.GetArtistsAndRecords2();
            // _rd.GetTotals();
            // _rd.NoRecordReviews();
            // _rd.GetRecordedYearDiscCount(1974);
            // _rd.GetFormattedRecord(212);
            // _rd.GetArtistNumberOfRecords(114);
            _rd.CountDiscs();
            // _rd.InsertRecord();
            // _rd.UpdateRecord();
            // _rd.GetRecord(1135);
            // _rd.DeleteRecord(5253);
        }
    }
}
