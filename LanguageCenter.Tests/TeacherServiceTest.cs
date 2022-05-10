using LanguageCenter.Core.Services;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCenter.Tests
{
    [TestFixture]
    internal class TeacherServiceTest
    {
        private const string teacherNotFound = "teacher not found.";

        private const int _page = 1;

        private List<Language> _languages;

        private List<Teacher> _teachers;

        private List<ApplicationUser> _users;

        private readonly ITeacherService _teacherService;

        private readonly Mock<IApplicationRepository> _applicationRepository;

        public TeacherServiceTest()
        {
            _applicationRepository = new();
            _teacherService = new TeacherService(_applicationRepository.Object);
        }

        [SetUp]
        public void Setup()
        {
            var user1 = new ApplicationUser()
            {
                Id = "123456789",
                FirstName = "Alexander",
                LastName = "Milanov",
                Email = "alex@abv.bg",
            };

            var user2 = new ApplicationUser()
            {
                Id = "1234567890",
                FirstName = "Dimitur",
                LastName = "Dimitrov",
                Email = "mitko@abv.bg",
            };

            var english = new Language()
            {
                Id = "1234",
                Name = "english",
                NormalizedName = "ENGLISH"
            };

            var spanish = new Language()
            {
                Id = "12345",
                Name = "spanish",
                NormalizedName = "SPANISH"
            };

            var french = new Language()
            {
                Id = "123456",
                Name = "french",
                NormalizedName = "FRENCH"
            };

            _languages = new List<Language>()
            {
                english,
                spanish,
                french
            };

            _users = new List<ApplicationUser>()
            {
                user1,
                user2
            };

            _teachers = new List<Teacher>()
            {
                new Teacher()
                {
                    Id = "1",
                    UserId = user1.Id,
                    User = user1,
                    Languages = new List<Language>()
                    {
                        english
                    }
                },
                new Teacher()
                {
                    Id = "2",
                    UserId = user2.Id,
                    User = user2,
                    Languages = new List<Language>()
                }
            };
        }

        [Test]
        public void AddLanguagesToTeacher_Throws_IfTeacherNotFound()
        {
            var languagesMock = _languages.BuildMock();
            var teachersMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languagesMock);
            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teachersMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _teacherService.AddLanguagesToTeacher(
                    "invalid",
                    new List<string>()));

            Assert.AreEqual(teacherNotFound, ex?.Message);
        }

        [Test]
        public void AddLanguagesToTeacher_Throws_IfCantSaveChanges()
        {
            var languagesMock = _languages.BuildMock();
            var teachersMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languagesMock);
            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teachersMock);
            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(() => throw new DbUpdateException());

            Assert.CatchAsync<DbUpdateException>(async () =>
                await _teacherService.AddLanguagesToTeacher(_teachers[0].Id, new List<string>() { "ENGLISH" }));
        }

        [Test]
        public void AddLanguagesToTeacher_Success()
        {
            var languagesMock = _languages.BuildMock();
            var teachersMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languagesMock);
            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teachersMock);

            Assert.DoesNotThrowAsync(async () =>
                await _teacherService.AddLanguagesToTeacher(_teachers[0].Id, new List<string>() { "ENGLISH" }));
        }

        [Test]
        public void RemoveLanguagesFromTeacher_Throws_IfTeacherNotFound()
        {
            var teachersMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teachersMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _teacherService.RemoveLanguagesFromTeacher("invalid"));
            Assert.AreEqual(teacherNotFound, ex?.Message);
        }

        [Test]
        public void RemoveLanguagesFromTeacher_Success()
        {
            var teachersMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teachersMock);
            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            Assert.DoesNotThrowAsync(async () =>
                await _teacherService.RemoveLanguagesFromTeacher(_teachers[0].Id));
        }

        [Test]
        public async Task GetAllTeachers_ReturnsCollection()
        {
            var teachersMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teachersMock);

            var actual = await _teacherService.GetAllTeachers(_page);

            var empty = await _teacherService.GetAllTeachers(100);

            CollectionAssert.IsNotEmpty(actual.Teachers);
            CollectionAssert.IsEmpty(empty.Teachers);
        }
    }
}
