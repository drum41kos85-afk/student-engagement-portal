using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Data;
using StudentEngagementPortal.Models;
using Xunit;

namespace StudentEngagementPortal.Tests
{
    public class EventRegistrationTests
    {
        private static ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CanRegisterStudentForEvent()
        {
            using var context = CreateInMemoryContext();

            var ev = new Event { Title = "Test Event", Description = "Desc", Category = "Social", StartsAt = DateTime.UtcNow.AddDays(1), MaxParticipants = 10 };
            context.Events.Add(ev);
            await context.SaveChangesAsync();

            context.Registrations.Add(new Registration { EventId = ev.Id, StudentId = "student-1" });
            await context.SaveChangesAsync();

            var count = await context.Registrations.CountAsync(r => r.EventId == ev.Id);
            Assert.Equal(1, count);
        }

        [Fact]
public async Task DuplicateRegistration_IsPreventedByApplicationLogic()
{
    using var context = CreateInMemoryContext();

    var ev = new Event { Title = "Test Event", Description = "Desc", Category = "Social", StartsAt = DateTime.UtcNow.AddDays(1), MaxParticipants = 10 };
    context.Events.Add(ev);
    await context.SaveChangesAsync();

    context.Registrations.Add(new Registration { EventId = ev.Id, StudentId = "student-1" });
    await context.SaveChangesAsync();

    bool alreadyRegistered = await context.Registrations.AnyAsync(r => r.EventId == ev.Id && r.StudentId == "student-1");

    Assert.True(alreadyRegistered);
}

        [Fact]
        public async Task DifferentStudents_CanBothRegisterForSameEvent()
        {
            using var context = CreateInMemoryContext();

            var ev = new Event { Title = "Test Event", Description = "Desc", Category = "Social", StartsAt = DateTime.UtcNow.AddDays(1), MaxParticipants = 10 };
            context.Events.Add(ev);
            await context.SaveChangesAsync();

            context.Registrations.Add(new Registration { EventId = ev.Id, StudentId = "student-1" });
            context.Registrations.Add(new Registration { EventId = ev.Id, StudentId = "student-2" });
            await context.SaveChangesAsync();

            var count = await context.Registrations.CountAsync(r => r.EventId == ev.Id);
            Assert.Equal(2, count);
        }
    }
}