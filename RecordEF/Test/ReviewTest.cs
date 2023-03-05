using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RecordEF.Data;
using RecordEF.Models;
using System.Globalization;
using _ad = RecordEF.Data.ArtistData;
using _revd = RecordEF.Data.ReviewData;


namespace RecordEF.Test
{
    public class ReviewTest
    {
        internal static void GetReviews()
        {
            var artistRecords = _ad.GetArtists().
                GroupJoin(
                    _revd.GetReviews(),
                    a => a.ArtistId,
                    r => r.ArtistId,
                    (a, r) => new { a, r }
                );

            foreach (var artist in artistRecords)
            {
                if (_revd.GetReviews().Select(r => r.ArtistId).Contains(artist.a.ArtistId))
                {
                    Console.WriteLine($"\n{artist.a.Name}\n");
                }

                foreach (var review in artist.r)
                {
                    Console.WriteLine($"\t{review.RecordName} - {review.Author} ({review.Published})");
                }
            }
        }

        internal static void GetArtistsAndReviews() 
        {
            var artistReviews = _revd.GetArtistsAndReviews();

            var artists = artistReviews
                .GroupBy(ar => new { ar.ArtistId, ar.Name })
                .Select(g => new Artist { ArtistId = g.Key.ArtistId, Name = g.Key.Name })
                .ToList();

            foreach (var artist in artists)
            {
                Console.WriteLine($"{artist.ArtistId} - {artist.Name}");
                var reviews = artistReviews.Where(ar => ar.ArtistId == artist.ArtistId);
                foreach (var review in reviews)
                {
                    Console.WriteLine($"\t{review.RecordName} - {review.Author} ({review.Published})");
                }
            }
        }

        internal static void GetArtistAndReviews(string artistName)
        {
            var artistReviews = _revd.GetArtistsAndReviews();

            var artists = artistReviews
                .GroupBy(ar => new { ar.ArtistId, ar.Name })
                .Select(g => new Artist { ArtistId = g.Key.ArtistId, Name = g.Key.Name })
                .Where(a => a.Name.ToLower().Contains(artistName.ToLower()))
                .ToList();

            if (artists.Count == 0)
            {
                Console.WriteLine($"No artists found with name '{artistName}'.");
            }
            else
            {
                foreach (var artist in artists)
                {
                    TextInfo name = CultureInfo.CurrentCulture.TextInfo;
                    var artistProperCase = name.ToTitleCase(artist.Name);

                    Console.WriteLine($"{artist.ArtistId} - {artistProperCase}");

                    var reviews = artistReviews.Where(ar => ar.ArtistId == artist.ArtistId);
                    foreach (var review in reviews)
                    {
                        Console.WriteLine($"\t{review.RecordName} - {review.Author} ({review.Published})");
                    }
                }
            }
        }
    }
}
