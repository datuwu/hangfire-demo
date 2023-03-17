using AutoMapper;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteManagementAPI.Controllersnote;
using NoteManagementAPI.DAL;
using NoteManagementAPI.Entities;
using NoteManagementAPI.Models.MappingProfiles;

namespace NoteManagentTest
{
    public class NotesControllerTest
    {
        private readonly Mock<UnitOfWork> _mockUnitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<NoteAppDbContext> _appDbContext;

        private readonly NotesController _controller;

        public NotesControllerTest()
        {
            _mockUnitOfWork = new Mock<UnitOfWork>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            _mapper = mappingConfig.CreateMapper();

            _appDbContext = new Mock<NoteAppDbContext>();
            _controller = new NotesController(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public void GetNotes_ActionExecutes_ReturnListOfNotes()
        {
            var result = _controller.GetNotes();

            Assert.IsType<Task<ActionResult<IEnumerable<Note>>>>(result);
        }

        [Fact]
        public void GetNotes_ActionExecutes_ReturnExactNumberOfNotes()
        {
            _mockUnitOfWork.Setup(uot => uot.NoteRepo.Get(null, null, ""))
                .Returns(new List<Note>() { new Note(), new Note() });

            var result = _controller.GetNotes();

            Assert.IsType<Task<ActionResult<IEnumerable<Note>>>>(result);
            Assert.Equal(2, result.Result.Value.Count());
        }
    }
}