using Microsoft.EntityFrameworkCore;
using RecordEF.Data;
using RecordEF.Models;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using _at = RecordEF.Test.ArtistTest;
using _rt = RecordEF.Test.RecordTest;
using _ret = RecordEF.Test.ReviewTest;

namespace RecordEF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //// Artists
            // _at.CreateArtist();
            // _at.GetArtistById(114);
            // _at.GetArtistEntity(114);
            // _at.GetArtistByName("Bob Dylan");
            _at.GetArtistNameByRecordId(2196);
            // _at.GetBiography(114);
            // _at.ArtistHtml(114);
            // _at.GetArtistId("Bob", "Dylan");
            // _at.GetArtistId(2196);
            // _at.UpdateArtist(836);
            // _at.DeleteArtist(836);
            // _at.GetArtist(114);
            // _at.GetArtistName(114);
            // _at.GetArtistNames();
            // _at.GetArtistsWithNoBio();
            // _at.GetNoBiographyCount();
            // _at.GetArtistDropdownList();

            //// Records
            // _rt.CreateRecord(833);
            //_rt.UpdateRecord(5257);
            // _rt.DeleteRecord(5257);
            // _rt.GetRecordById(2196);
            // _rt.Select(2196);
            // _rt.GetRecordEntity(2196);
            // _rt.GetArtistRecordByYear(1974);
            // _rt.GetRecordList();
            // _rt.GetRecordList2();
            // _rt.GetRecordList3();
            // _rt.CountDiscs(string.Empty);
            // _rt.CountDiscs("DVD");
            // _rt.CountDiscs("CD");
            // _rt.CountDiscs("R");
            // _rt.GetArtistRecordEntity(2196);
            // _rt.GetArtistByRecordId(2196);
            // _rt.GetArtistRecords(114);
            // _rt.GetArtistNumberOfRecords(114);
            // _rt.GetRecordDetails(2196);
            // _rt.GetArtistNameFromRecord(2196);
            // _rt.GetDiscCountForYear(1974);
            // _rt.GetBoughtDiscCountForYear(1995);
            // _rt.GetNoRecordReview();
            // _rt.GetNoReviewCount();
            // _rt.GetTotalArtistCost();
            // _rt.GetTotalArtistDiscs();
            // _rt.RecordHtml(2196);
            // _rt.GetRecordDropdownList(114);

            //// Reviews
            // _ret.GetReviews(); // painfully slow.
            // _ret.GetArtistsAndReviews();
            // _ret.GetArtistAndReviews("Bob Dylan");
        }
    }
}
