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
    [Fact]
    public void Render_ShouldReturnValidComponent()
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
                            2,
                            new AssignmentPoints(Guid.Empty, DateOnly.MinValue, false, 0d)),
                    }),
            });

        var table = new PartialPointsTable(new RuCultureInfoProvider());

        // Act
        IComponent component = table.Render(points);

        // Assert
        component.Size.Height.Should().Be(4);
        component.Size.Width.Should().Be(11);
    }
}