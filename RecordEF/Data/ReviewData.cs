using Microsoft.EntityFrameworkCore;
using RecordEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordEF.Data
{
    public class ReviewData
    {
        public static List<Review> GetReviews()
        {
            var list = new List<Review>();
            using (var context = new RecordDbContext())
            {
                return list = context.Reviews.OrderBy(r => r.Name).ThenBy(r => r.RecordName).ToList();

            }
        }

        public static List<dynamic> GetArtistsAndReviews()
        {
            using (var context = new RecordDbContext())
            {
                return context.Reviews
                            .Join(
                                context.Artists,
                                r => r.ArtistId,
                                a => a.ArtistId,
                                (r, a) => new { Review = r, Artist = a }
                            )
                            .Select(ra => new
                            {
                                ra.Review.PitchforkId,
                                ra.Review.ReviewId,
                                ra.Review.ArtistId,
                                ArtistName = ra.Artist.Name,
                                ra.Review.RecordId,
                                ra.Review.Name,
                                ra.Review.RecordName,
                                ra.Review.Author,
                                ra.Review.Published,
                                ra.Review.Review1
                            })
                            .Where(r => r.ArtistId > 0 && !string.IsNullOrEmpty(r.ArtistName))
                            .OrderBy(r => r.ArtistName)
                            .Cast<dynamic>()
                            .ToList();
            }

        }

        public static List<dynamic> GetArtistReviews(string artistName)
        {
            using (var context = new RecordDbContext())
            {
                return context.Reviews
                            .Join(
                                context.Artists,
                                r => r.ArtistId,
                                a => a.ArtistId,
                                (r, a) => new { Review = r, Artist = a }
                            )
                            .Select(ra => new
                            {
                                ra.Review.PitchforkId,
                                ra.Review.ReviewId,
                                ra.Review.ArtistId,
                                ArtistName = ra.Artist.Name,
                                ra.Review.RecordId,
                                ra.Review.Name,
                                ra.Review.RecordName,
                                ra.Review.Author,
                                ra.Review.Published,
                                ra.Review.Review1
                            })
                            .Where(r => r.ArtistName == artistName)
                            .OrderByDescending(r => r.Published)
                            .Cast<dynamic>()
                            .ToList();
            }
        }
    }
}


