using System;
using Business.Handlers.Handlers.contracts;
using Business.Handlers.Request;
using Business.Handlers.Response;
using Common.DTOs;
using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Musical.Broccoli.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRequestHandler _requestHandler;

        public UserController(IUserRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }

        [HttpGet]
        public IActionResult Get([FromBody] ReadRequest request)
        {
            Response<UserDTO> result;
            try
            {
                result = _requestHandler.HandleReadRequest(request);
            }
            catch (UnauthorizedAccessException)
            {
                return new ForbidResult();
            }
            catch (InvalidRequestException)
            {
                return new BadRequestResult();
            }

            if (result.Data.Count <= 0)
            {
                return new NotFoundObjectResult(result);
            }
            return new OkObjectResult(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ReadWriteRequest<UserDTO> request)
        {
            Response<UserDTO> result;

            try
            {
                result = _requestHandler.HandleReadWriteRequest(request);
            }
            catch (InternalServerErrorException)
            {
                return StatusCode(500);
            }
            catch (AuthenticationException)
            {
                return new ForbidResult();
            }
            catch (InvalidRequestException)
            {
                return new BadRequestResult();
            }

            return new CreatedResult("", result);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] ReadWriteRequest<UserDTO> request)
        {
            Response<SessionDTO> result;

            try
            {
                result = _requestHandler.HandleLoginRequest(request);
            }
            catch (UnauthorizedAccessException)
            {
                return new ForbidResult();
            }
            catch (InvalidRequestException)
            {
                return new BadRequestResult();
            }

            if (result.Data.Count <= 0)
            {
                return new NotFoundObjectResult(result);
            }

            return new OkObjectResult(result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] ReadWriteRequest<UserDTO> request)
        {
            try
            {
                _requestHandler.HandleReadWriteRequest(request);
            }
            catch (AuthenticationException)
            {
                return new ForbidResult();
            }
            catch (InvalidRequestException)
            {
                return new BadRequestResult();
            }

            return new NoContentResult();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] ReadWriteRequest<UserDTO> request)
        {
            try
            {
                _requestHandler.HandleDeleteRequest(request);
            }
            catch (AuthenticationException)
            {
                return new ForbidResult();
            }
            catch (InvalidRequestException)
            {
                return new BadRequestResult();
            }

            return new NoContentResult();
        }
    }
}