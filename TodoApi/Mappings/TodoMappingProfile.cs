using AutoMapper;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Models;

namespace TodoApi.Mappings;

public class TodoMappingProfile : Profile
{
    public TodoMappingProfile()
    {
        CreateMap<CreateTodoDto, TodoItem>();

        CreateMap<UpdateTodoDto, TodoItem>();

        CreateMap<TodoItem, TodoResponseDto>();
    }
}