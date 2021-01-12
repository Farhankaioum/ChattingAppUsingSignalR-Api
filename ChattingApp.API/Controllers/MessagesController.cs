using AutoMapper;
using ChattingApp.API.Dtos;
using ChattingApp.Foundation.Entities;
using ChattingApp.Foundation.Helpers;
using ChattingApp.Foundation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ChattingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessageService messageService,
            IUserService userService,
            IMapper mapper,
            ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public IActionResult GetMessage(Guid userId, long id)
        {
            if (userId != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var existingMsg = _messageService.GetMessage(id);

            if (existingMsg == null)
                return NotFound();

            var returnMsg = _mapper.Map<MessageToReturnDto>(existingMsg);

            return Ok(returnMsg);
        }

        [HttpGet]
        public IActionResult GetMessagesForUser(Guid userId, [FromQuery] MessageParams messageParams)
        {
            if (userId != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo =  _messageService.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public ActionResult GetMessageThread(Guid userId, Guid recipientId)
        {
            if (userId != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = _messageService.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public IActionResult CreateMessage(Guid userId, MessageForCreationDto messageForCreationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var sender = _userService.GetUserById(userId);

                if (sender.Id != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                messageForCreationDto.SenderId = userId;

                var recipient = _userService.GetUserById(messageForCreationDto.RecipientId);

                if (recipient == null)
                    return BadRequest("Could not find recipient user");

                var message = _mapper.Map<Message>(messageForCreationDto);

                _messageService.AddMessage(message);

                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);

                return CreatedAtRoute("GetMessage", new { id = message.Id, userId }, messageToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest();
            }
            
        }

        [HttpPost("{id}")]
        public IActionResult DeleteMessage(long id, Guid userId)
        {
            if (userId != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try
            {
                var messageFromRepo = _messageService.GetMessage(id);

                if (messageFromRepo == null)
                    return NotFound();

                if (messageFromRepo.SenderId == userId)
                    messageFromRepo.SenderDeleted = true;

                if (messageFromRepo.RecipientId == userId)
                    messageFromRepo.RecipientDeleted = true;

                if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                    _messageService.DeleteMessage(id, userId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("{id}/read")]
        public IActionResult MarkMessageAsRead(Guid userId, long id)
        {
            if (userId != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try
            {
                var message = _messageService.GetMessage(id);

                if (message == null)
                    return NotFound();

                if (message.RecipientId != userId)
                    return Unauthorized();

                message.IsRead = true;
                message.DateRead = DateTime.Now;

                _messageService.MarkMessageAsRead(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            
        }
    }
}
