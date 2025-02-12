﻿using Ecom.API.Errors;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private ApplicationDbContext _context;

        public BugController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            var product = _context.Products.Find(0);
            if (product == null)
            {
                return NotFound(new BaseCommonResponse(404));
            }
            return Ok(product);

        }

        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(0);
            product.Name = "";
            return Ok();
            //var problemDetails = new ProblemDetails
            //{
            //    Status = 500,
            //    Title = "Internal Server Error",
            //    Detail = "An unexpected error occurred while processing the request."
            //};

            //return StatusCode(500, problemDetails);
        }

        [HttpGet("bad-request/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();

        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new BaseCommonResponse(400));

        }
    }
}
