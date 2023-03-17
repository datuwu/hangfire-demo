using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteManagementAPI.DAL;
using NoteManagementAPI.Entities;
using NoteManagementAPI.Models;

namespace NoteManagementAPI.Controllersnote
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotesController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return Ok(_unitOfWork.NoteRepo.Get());
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotesByUser(int userId)
        {
            var notes = _unitOfWork.NoteRepo.Get(filter: n => n.UserId == userId);
            return Ok(notes);
        }


        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = _unitOfWork.NoteRepo.GetById(id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // PUT: api/Notes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, NoteUpdateVM noteVM)
        {
            try
            {
                if (id != noteVM.Id)
                {
                    return BadRequest();
                }
                var note = _unitOfWork.NoteRepo.GetById(id);
                var updateNote = _mapper.Map(noteVM, note);
                _unitOfWork.NoteRepo.Update(updateNote);

                _unitOfWork.Save();
                return Ok(updateNote);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
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

        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(NoteVM noteVM)
        {
            var note = _mapper.Map<Note>(noteVM);
            _unitOfWork.NoteRepo.Insert(note);
            _unitOfWork.Save();

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = _unitOfWork.NoteRepo.GetById(id);
            if (note == null)
            {
                return NotFound();
            }

            _unitOfWork.NoteRepo.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _unitOfWork.NoteRepo.Get().Any(e => e.Id == id);
        }
    }
}
