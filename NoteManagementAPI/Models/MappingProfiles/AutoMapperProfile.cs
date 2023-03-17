using AutoMapper;
using NoteManagementAPI.Entities;

namespace NoteManagementAPI.Models.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateRoleMap();
            CreateUserMap();
            CreateNoteMap();
        }

        private void CreateRoleMap()
        {
            CreateMap<RoleVM, Role>().ReverseMap();
            CreateMap<RoleUpdateVM, Role>().ReverseMap();

        }

        private void CreateUserMap()
        {
            CreateMap<UserRegisterVM, User>().ReverseMap();
            CreateMap<UserUpdateVM, User>().ReverseMap();
        }

        private void CreateNoteMap()
        {
            CreateMap<NoteVM, Note>().ReverseMap();
            CreateMap<NoteUpdateVM, Note>().ReverseMap();
        }
    }
}
