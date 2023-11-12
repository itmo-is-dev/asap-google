using FluentAssertions;
using FluentSpreadsheets;
using Itmo.Dev.Asap.Google.Application.Models.Tables.PartialPoints;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Points;
using Itmo.Dev.Asap.Google.Application.PartialPoints;
using Itmo.Dev.Asap.Google.Application.Providers;
using Xunit;

namespace Itmo.Dev.Asap.Google.Application.Tests.PartialPoints;

public class PartialPointsTableTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Render_ShouldReturnValidComponent(int assignmentOrdinal)
    {
        // Arrange
        var points = new PartialSubjectCoursePoints(
            3,
            new[]
            {
                new PartialStudentAssignmentPoints(
                    0,
                    new[]
                    {
                        new PartialAssignmentPoints(
                            assignmentOrdinal,
                            new AssignmentPoints(Guid.Empty, DateOnly.MinValue, false, 0d)),
                    }),
            });

        var table = new PartialPointsTable(new CultureInfoProvider());

        // Act
        IComponent component = table.Render(points);

        // Assert
        component.Size.Height.Should().Be(4);
        component.Size.Width.Should().Be(11);
    }
}