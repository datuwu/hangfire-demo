using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteManagementAPI.DAL;
using NoteManagementAPI.Entities;
using NoteManagementAPI.Models;

namespace NoteManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolesController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Roles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return Ok(_unitOfWork.RoleRepo.Get());
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = _unitOfWork.RoleRepo.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRole(int id, RoleUpdateVM roleVM)
        {
            try
            {
                if (id != roleVM.Id)
                {
                    return BadRequest();
                }
                var role = _unitOfWork.RoleRepo.GetById(id);
                var updateRole = _mapper.Map(roleVM, role);
                _unitOfWork.RoleRepo.Update(updateRole);

                _unitOfWork.Save();
                return Ok(updateRole);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Role>> PostRole(RoleVM roleVM)
        {
            var role = _mapper.Map<Role>(roleVM);
            _unitOfWork.RoleRepo.Insert(role);
            _unitOfWork.Save();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = _unitOfWork.RoleRepo.GetById(id);
            if (role == null)
            {
                return NotFound();
            }

            _unitOfWork.RoleRepo.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _unitOfWork.RoleRepo.Get().Any(e => e.Id == id);
        }
    }
}
