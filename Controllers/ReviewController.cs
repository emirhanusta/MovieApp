using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.Abstract;
using MovieApp.Entities;

namespace MovieApp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ILikeRepository _likeRepository;

        public ReviewController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(long reviewId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var like = await _likeRepository.Likes.FirstOrDefaultAsync(l => l.ReviewId == reviewId && l.UserId == Convert.ToInt64(userId));

            if (like != null)
            {
                _likeRepository.DeleteLike(like);
            }
            else
            {
                _likeRepository.SaveLike(new Like
                {
                    ReviewId = reviewId,
                    UserId = Convert.ToInt64(userId)
                });
            }
            var likeCount = await _likeRepository.Likes.CountAsync(l => l.ReviewId == reviewId);
            return Json(new { likeCount });
        }

    }
}