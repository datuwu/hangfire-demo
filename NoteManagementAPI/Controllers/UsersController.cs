using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteManagementAPI.DAL;
using NoteManagementAPI.Entities;
using NoteManagementAPI.Models;
using Hangfire;
using Castle.Core.Smtp;
using NoteManagementAPI.EmailSender;

namespace NoteManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(_unitOfWork.UserRepo.Get());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = _unitOfWork.UserRepo.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _unitOfWork.UserRepo.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            _unitOfWork.UserRepo.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdateVM userVM)
        {
            try
            {
                if (id != userVM.Id)
                {
                    return BadRequest();
                }
                var user = _unitOfWork.UserRepo.GetById(id);
                var updateUser = _mapper.Map(userVM, user);
                _unitOfWork.UserRepo.Update(updateUser);

                _unitOfWork.Save();
                return Ok(updateUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                    return BadRequest();
                }
            }
        }

        [HttpPut("disable/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DisableUser(int id)
        {
            var user = _unitOfWork.UserRepo.GetById(id);

            if (user is null) return BadRequest();

            user.Status = UserStatus.Disabled;
            _unitOfWork.Save();

            return Ok();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterVM userVM)
        {
            var user = _mapper.Map<User>(userVM);
            _unitOfWork.UserRepo.Insert(user);
            _unitOfWork.Save();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("changeUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> ChangeUserRole(int id, int roleId)
        {
            var user = _unitOfWork.UserRepo.GetById(id);
            var role = _unitOfWork.RoleRepo.GetById(roleId);
            user.RoleId = roleId;
            user.Role = role;

            _unitOfWork.Save();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpGet("sendMail")]
        public IActionResult SendEmailToAllUser()
        {
            RecurringJob.AddOrUpdate<EmailJob>("sendemail", x => x.SendEmail(), Cron.Hourly, TimeZoneInfo.Local);

            return Ok();
        }
        private bool UserExists(int id)
        {
            return _unitOfWork.UserRepo.Get().Any(e => e.Id == id);
        }
    }
}
