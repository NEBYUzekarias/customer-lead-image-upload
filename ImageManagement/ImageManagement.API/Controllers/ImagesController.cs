using ImageManagement.Application.Features.Images.CQRS.Commands;
using ImageManagement.Application.Features.Images.CQRS.Queries;
using ImageManagement.Application.Features.Images.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ImageManagement.API.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImagesController : BaseApiController
    {
        public ImagesController(IMediator mediator) : base(mediator) {}

        /// <summary>
        /// Upload one or more images to a customer or lead profile
        /// </summary>
        /// <param name="request">Upload request containing images and target entity</param>
        /// <returns>Upload result</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] UploadImagesRequest request)
        {
            var command = new UploadImagesCommand { Request = request };
            var result = await _mediator.Send(command);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        /// <summary>
        /// Get all images for a customer or lead
        /// </summary>
        /// <param name="customerId">Customer ID (optional)</param>
        /// <param name="leadId">Lead ID (optional)</param>
        /// <returns>List of images</returns>
        [HttpGet]
        public async Task<IActionResult> GetImages([FromQuery] int? customerId, [FromQuery] int? leadId)
        {
            if ((customerId == null && leadId == null) || (customerId != null && leadId != null))
            {
                return BadRequest("Provide either customerId or leadId, but not both.");
            }

            var query = new GetImagesQuery { CustomerId = customerId, LeadId = leadId };
            var images = await _mediator.Send(query);
            return Ok(images);
        }

        /// <summary>
        /// Delete an image from a customer or lead profile
        /// </summary>
        /// <param name="id">Image ID</param>
        /// <param name="customerId">Customer ID (optional)</param>
        /// <param name="leadId">Lead ID (optional)</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int? customerId, [FromQuery] int? leadId)
        {
            var command = new DeleteImageCommand 
            { 
                ImageId = id, 
                CustomerId = customerId, 
                LeadId = leadId 
            };
            
            var result = await _mediator.Send(command);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

    }
}
