using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticRepository _statisticRepository;
        public StatisticController(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }

        [HttpGet("likes-received/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LikesReceivedPerUser(int userId)
        {
            try
            {
                var result = await _statisticRepository.NumberOfLikesReceivedPerUser(userId);
                return Ok(new { status = "success", message = result });

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("comments-received/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CommentsReceivedPerUser(int userId)
        {
            try
            {
                var result = await _statisticRepository.NumberOfCommentsReceivedPerUser(userId);
                return Ok(new { status = "success", message = result });

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("likes-done/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LikesDonePerUser(int userId)
        {
            try
            {
                var result = await _statisticRepository.NumberOfLikesDonePerUser(userId);
                return Ok(new { status = "success", message = result });

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("comments-done/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CommentsDonePerUser(int userId)
        {
            try
            {
                var result = await _statisticRepository.NumberOfCommentsDonePerUser(userId);
                return Ok(new { status = "success", message = result });

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }
    }
}