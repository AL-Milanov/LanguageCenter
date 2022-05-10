using LanguageCenter.Core.Models.TeacherModels;
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
        private const string _teacherNotFound = "teacher not found.";

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
                    },
                    IsActive = true,
                },
                new Teacher()
                {
                    Id = "2",
                    UserId = user2.Id,
                    User = user2,
                    Languages = new List<Language>(),
                    IsActive = false,
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

            Assert.AreEqual(_teacherNotFound, ex?.Message);
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
            Assert.AreEqual(_teacherNotFound, ex?.Message);
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

        [Test]
        public void GetTeacher_Throws_IfTeacherNotFound()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _teacherService.GetTeacher("invalid"));

            Assert.AreEqual(_teacherNotFound, ex?.Message);
        }

        [Test]
        public async Task GetTeacher_Success_ReturnsTeacher()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            var actual = await _teacherService.GetTeacher(_teachers[0].Id);

            Assert.NotNull(actual);
            Assert.AreEqual(_teachers[0].Id, actual.Id);
        }

        [Test]
        public async Task GetTeachersId_Success_ReturnsCollection()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            var actual = (List<string>)await _teacherService.GetTeachersId();

            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(string));
            CollectionAssert.IsNotEmpty(actual);

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(_teachers[i].UserId, actual[i]);
            }
        }

        [Test]
        public void MakeActive_Throws_IfTeacherNotFound()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _teacherService.MakeActive("not-existing"));

            Assert.AreEqual(_teacherNotFound, ex?.Message);
        }

        [Test]
        public async Task MakeActive_Success()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var before = _teachers[1].IsActive;

            var actual = await _teacherService.MakeActive(_teachers[1].Id);

            Assert.AreNotEqual(before, actual);
        }

        [Test]
        public void MakeInactive_Throws_IfTeacherNotFound()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _teacherService.MakeInactive("not-existing"));

            Assert.AreEqual(_teacherNotFound, ex?.Message);
        }

        [Test]
        public async Task MakeInactive_Success()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var before = _teachers[0].IsActive;

            var actual = await _teacherService.MakeInactive(_teachers[0].Id);

            Assert.AreNotEqual(before, actual);
        }

        [Test]
        public void MakeTeacher_Throws_IfUserNotFound()
        {
            ApplicationUser user = null;

            _applicationRepository.Setup(x => x.GetByIdAsync<ApplicationUser>(It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _teacherService.MakeTeacher("not-existing"));

            Assert.AreEqual("user not found.", ex?.Message);
        }

        [Test]
        public async Task MakeTeacher_Success()
        {

            var userNotTeacher = new ApplicationUser()
            {
                Id = "valid-id",
                FirstName = "Valid",
                LastName = "User",
                Email = "validuser@gmail.com"
            };

            _users.Add(userNotTeacher);

            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetByIdAsync<ApplicationUser>(It.IsAny<string>()))
                .Returns(Task.FromResult(userNotTeacher));

            var expected = true;

            var actual = await _teacherService.MakeTeacher(userNotTeacher.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAllActiveTeachers_ReturnsCollection()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            var actual = await _teacherService.GetAllActiveTeachers();

            Assert.AreNotEqual(_teachers.Count, actual.Count);
        }

        [Test]
        public void EditDescription_AsUser_Throws_IfTeacherNotFound()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _teacherService.EditDescription(_teachers[1].Id, _teachers[1].UserId, "new description"));

        }

        [Test]
        public void EditDescription_AsUser_Throws_IfUserIsNotTeacher()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            Assert.CatchAsync<UnauthorizedAccessException>(async () =>
                await _teacherService.EditDescription(_teachers[0].Id, _teachers[1].UserId, "new description"));

        }

        [Test]
        public async Task EditDescription_AsUser_Success()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);
            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var newDescription = "changed description";

            await _teacherService.EditDescription(_teachers[0].Id, _teachers[0].UserId, newDescription);

            Assert.AreEqual(newDescription, _teachers[0].Description);
        }

        [Test]
        public void EditDescription_AsAdmin_Throws_IfTeacherNotFound()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _teacherService.EditDescription(_teachers[1].Id, "new description"));

        }

        [Test]
        public async Task EditDescription_AsAdmin_Success()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);
            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var newDescription = "changed description as admin";

            await _teacherService.EditDescription(_teachers[0].Id, newDescription);

            Assert.AreEqual(newDescription, _teachers[0].Description);
        }

        [Test]
        public void GetTeacherDescription_Throws_IfTeacherNotFound()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            Assert.CatchAsync<ArgumentException>(async () => 
                await _teacherService.GetTeacherDescription("not-existing"));
        }

        [Test]
        public async Task GetTeacherDescription_Success()
        {
            var teacherMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            var actual = await _teacherService.GetTeacherDescription(_teachers[0].Id);

            Assert.IsAssignableFrom<TeacherDescriptionVM>(actual);
            Assert.AreEqual(_teachers[0].Id, actual.TeacherId);
        }
    }
}
