using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.requests;
using Backend.Interfaces;
using Backend.Models.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("number-comments/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<int>> NumberOfCommentsPerImage(int imageId)
        {
            try
            {
                var result = await _commentRepository.NumberOfCommentsPerImage(imageId);
                return Ok(new { status = "success", message = result });

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("comments-per-image/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<Comment>>> CommentsPerImage(int imageId)
        {
            try
            {
                var result = await _commentRepository.GetCommentPerImage(imageId);
                return Ok(new { status = "success", message = result });

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }


        [HttpPost("comment/{userId}/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<object>> CommentImage(int userId, int imageId, [FromBody] string commentText)
        {
            try
            {
                var result = await _commentRepository.CommentImage(userId, imageId, commentText);

                if (result)
                {
                    return Ok(new { status = "success", message = "Image commented successfully." });
                }

                return BadRequest("Error has occured");

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpDelete("uncomment/{userId}/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UncommentImage([FromBody] int userCommentId, int userId, int imageId)
        {
            try
            {

                var result = await _commentRepository.DeleteCommentImage(userId, imageId, userCommentId);

                if (result)
                {
                    return Ok(new { status = "success", message = "Image uncommented successfully." });
                }

                return BadRequest("User hasn't commented the image");

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpPut("update-comment/{userId}/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCommentImage([FromBody] ChangeCommentRequest changeCommentRequest, int userId, int imageId)
        {
            try
            {

                var result = await _commentRepository.UpdateCommentImage(userId, imageId, changeCommentRequest.UserCommentId, changeCommentRequest.NewComment);

                if (result)
                {
                    return Ok(new { status = "success", message = "Comment updated successfully." });
                }

                return BadRequest("User hasn't commented the image");

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

    }
}