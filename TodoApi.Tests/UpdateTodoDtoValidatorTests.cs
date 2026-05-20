using TodoApi.DTOs.TodoDTOs;
using TodoApi.Models.Enums;
using TodoApi.Validators;

namespace TodoApi.Tests;

public class UpdateTodoDtoValidatorTests
{
    private readonly UpdateTodoDtoValidator _validator = new();

    [Fact]
    public void Validate_WhenCategoryIsOutsideEnum_ReturnsValidationError()
    {
        var dto = new UpdateTodoDto
        {
            Title = "Update todo",
            Description = "Valid description",
            Priority = PriorityLevel.Medium,
            Category = (CategoryType)99,
            IsCompleted = true
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateTodoDto.Category));
    }

    [Fact]
    public void Validate_WhenDtoIsValid_ReturnsSuccess()
    {
        var dto = new UpdateTodoDto
        {
            Title = "Update todo",
            Description = "Valid description",
            Priority = PriorityLevel.Low,
            Category = CategoryType.Personal,
            IsCompleted = true
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
    }
}
