using TodoApi.DTOs.TodoDTOs;
using TodoApi.Models.Enums;
using TodoApi.Validators;

namespace TodoApi.Tests;

public class CreateTodoDtoValidatorTests
{
    private readonly CreateTodoDtoValidator _validator = new();

    [Fact]
    public void Validate_WhenTitleIsMissing_ReturnsValidationError()
    {
        var dto = new CreateTodoDto
        {
            Title = "",
            Description = "Details",
            Priority = PriorityLevel.Medium,
            Category = CategoryType.Work
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateTodoDto.Title));
    }

    [Fact]
    public void Validate_WhenPriorityIsOutsideEnum_ReturnsValidationError()
    {
        var dto = new CreateTodoDto
        {
            Title = "Write tests",
            Description = "Add backend coverage",
            Priority = (PriorityLevel)99,
            Category = CategoryType.Work
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateTodoDto.Priority));
    }

    [Fact]
    public void Validate_WhenDtoIsValid_ReturnsSuccess()
    {
        var dto = new CreateTodoDto
        {
            Title = "Write tests",
            Description = "Add backend coverage",
            Priority = PriorityLevel.High,
            Category = CategoryType.Education
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
    }
}
