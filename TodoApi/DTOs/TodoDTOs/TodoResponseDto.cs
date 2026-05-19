using TodoApi.Models.Enums;

namespace TodoApi.DTOs.TodoDTOs;

public class TodoResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public PriorityLevel Priority { get; set; }

    public CategoryType Category { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }
}