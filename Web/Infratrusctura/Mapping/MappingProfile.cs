using AutoMapper;
using Entity.DTO;
using Entity.DTOs;
using Entity.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Infrastructure.Mapping
{
    /// <summary>
    /// Perfil de AutoMapper para configurar los mapeos entre entidades y DTOs.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor que configura todos los mapeos.
        /// </summary>
        public MappingProfile()
        {
            // Mapeo Rol <-> RolDto
            CreateMap<Rol, RolDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameRol, opt => opt.MapFrom(src => src.NameRol))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<RolDto, Rol>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameRol, opt => opt.MapFrom(src => src.NameRol))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.RolForms, opt => opt.Ignore());

            // Mapeo Form <-> FormDto
            CreateMap<Form, FormDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameForm, opt => opt.MapFrom(src => src.NameForm))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.IdModule, opt => opt.MapFrom(src => src.IdModule));

            CreateMap<FormDto, Form>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameForm, opt => opt.MapFrom(src => src.NameForm))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.IdModule, opt => opt.MapFrom(src => src.IdModule))
                .ForMember(dest => dest.Module, opt => opt.Ignore())
                .ForMember(dest => dest.RolForms, opt => opt.Ignore());

            // Mapeo Module <-> ModuleDto
            CreateMap<Module, ModuleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameModule, opt => opt.MapFrom(src => src.NameModule))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.status));

            CreateMap<ModuleDto, Module>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameModule, opt => opt.MapFrom(src => src.NameModule))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.status))
                .ForMember(dest => dest.Forms, opt => opt.Ignore());

            // Mapeo Person <-> PersonDto
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.TypeIdentification, opt => opt.MapFrom(src => src.TypeIdentification))
                .ForMember(dest => dest.NumberIndification, opt => opt.MapFrom(src => src.NumberIndification))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<PersonDto, Person>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.TypeIdentification, opt => opt.MapFrom(src => src.TypeIdentification))
                .ForMember(dest => dest.NumberIndification, opt => opt.MapFrom(src => src.NumberIndification))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Mapeo User <-> UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreateAt))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => src.UpdateAt))
                .ForMember(dest => dest.DeleteAt, opt => opt.MapFrom(src => src.DeleteAt))
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.IdPerson, opt => opt.MapFrom(src => src.IdPerson))
                .ForMember(dest => dest.IdCompany, opt => opt.MapFrom(src => src.IdCompany));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreateAt))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => src.UpdateAt))
                .ForMember(dest => dest.DeleteAt, opt => opt.MapFrom(src => src.DeleteAt))
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.IdPerson, opt => opt.MapFrom(src => src.IdPerson))
                .ForMember(dest => dest.IdCompany, opt => opt.MapFrom(src => src.IdCompany))
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.Person, opt => opt.Ignore())
                .ForMember(dest => dest.Rol, opt => opt.Ignore())
                .ForMember(dest => dest.Buyouts, opt => opt.Ignore())
                .ForMember(dest => dest.Seles, opt => opt.Ignore())
                .ForMember(dest => dest.Notifications, opt => opt.Ignore());

            // Mapeo RolForm <-> RolFormDto
            CreateMap<RolForm, RolFormDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Permision, opt => opt.MapFrom(src => src.Permision))
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.IdForm, opt => opt.MapFrom(src => src.IdForm))
                .ForMember(dest => dest.Name, opt => opt.Ignore()); // No mapped directly from entity

            CreateMap<RolFormDto, RolForm>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Permision, opt => opt.MapFrom(src => src.Permision))
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.IdRol))
                .ForMember(dest => dest.IdForm, opt => opt.MapFrom(src => src.IdForm))
                .ForMember(dest => dest.Rol, opt => opt.Ignore())
                .ForMember(dest => dest.Form, opt => opt.Ignore());

            // Configuración especial para actualizaciones parciales
            // Este mapeo sólo actualiza propiedades no nulas
            CreateMap<RolDto, Rol>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<FormDto, Form>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ModuleDto, Module>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PersonDto, Person>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RolFormDto, RolForm>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}