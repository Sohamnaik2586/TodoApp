using TodoApi.Models.Enums;

namespace TodoApi.DTOs.TodoDTOs;

public class CreateTodoDto
{
	public string Title { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public PriorityLevel Priority { get; set; }

	public CategoryType Category { get; set; }
}