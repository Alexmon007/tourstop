using System;
using Business.Handlers.Handlers.contracts;
using Business.Handlers.Request;
using Business.Handlers.Response;
using Common.DTOs;
using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Musical.Broccoli.API.Controllers
{
    public class OrderController : Controller, IBaseController<OrderDTO>
    {
        private readonly IOrderRequestHandler _requestHandler;

        public OrderController( IOrderRequestHandler requestHandler )
        {
            _requestHandler = requestHandler;
        }


        [HttpGet]
        public IActionResult Get( [FromBody] ReadRequest request )
        {
            Response<OrderDTO> result;
            try
            {
                result = _requestHandler.HandleReadRequest( request );
            }
            catch (AuthenticationException)
            {
                return new ForbidResult();
            }
            catch (InvalidRequestException)
            {
                return new BadRequestResult();
            }

            if (result.Data.Count <= 0)
            {
                return new NotFoundObjectResult( result );
            }
            return new OkObjectResult( result );
        }

        [HttpPost]
        public IActionResult Post( [FromBody] ReadWriteRequest<OrderDTO> request )
        {
            Response<OrderDTO> result;

            try
            {
                result = _requestHandler.HandleReadWriteRequest( request );
            }
            catch (AuthenticationException)
            {
                return new ForbidResult();
            }
            catch (InvalidRequestException)
            {
                return new BadRequestResult();
            }

            return new CreatedResult( "", result );
        }

        [HttpPut]
        public IActionResult Put( [FromBody] ReadWriteRequest<OrderDTO> request )
        {
            try
            {
                _requestHandler.HandleReadWriteRequest( request );
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
        public IActionResult Delete( [FromBody] ReadWriteRequest<OrderDTO> request )
        {
            try
            {
                _requestHandler.HandleDeleteRequest( request );
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