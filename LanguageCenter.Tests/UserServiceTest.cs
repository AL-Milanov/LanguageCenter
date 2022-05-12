using LanguageCenter.Core.Services;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanguageCenter.Tests
{
    [TestFixture]
    internal class UserServiceTest
    {
        private const string _userNotFound = "user not found.";

        private List<ApplicationUser> _users;

        private List<Course> _courses;

        private readonly IUserService _userService;

        private readonly Mock<IApplicationRepository> _applicationRepository;

        public UserServiceTest()
        {
            _applicationRepository = new();
            _userService = new UserService(_applicationRepository.Object);
        }

        [SetUp]
        public void Setup()
        {
            var user1 = new ApplicationUser()
            {
                Id = "user1",
                FirstName = "Alexander",
                LastName = "Milanov",
                Email = "alex@gmail.com",
                Courses = new List<Course>()
                {
                    new Course
                    {
                        Id = "valid-course",
                        Title = "Английски за начинаещи",
                        EndDate = DateTime.Now,
                    },
                    new Course
                    {
                        Id = "inactive-valid-course",
                        Title = "Английски за начинаещи",
                        EndDate = DateTime.Parse("11/11/2021")
                    }

                }
            };

            var user2 = new ApplicationUser()
            {
                Id = "user1",
                FirstName = "Alexander",
                LastName = "Milanov",
                Email = "alex@gmail.com"
            };

            var user3 = new ApplicationUser()
            {
                Id = "user1",
                FirstName = "Alexander",
                LastName = "Milanov",
                Email = "alex@gmail.com"
            };

            _users = new()
            {
                user1,
                user2,
                user3
            };

            _courses = new()
            {
                new Course
                {
                    Id = "full-course",
                    Title = "Английски за начинаещи",
                    EndDate = DateTime.Now,
                    Capacity = 1,
                    Students = new List<ApplicationUser>()
                    {
                        user1
                    }
                },
                new Course
                {
                    Id = "inactive-valid-course",
                    Title = "Английски за начинаещи",
                    EndDate = DateTime.Parse("11/11/2021")
                }
            };
        }


        [Test]
        public async Task GetAll_Success_ReturnCollection()
        {
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            var collection = await _userService.GetAll(1, u => true);
            var empty = await _userService.GetAll(2, u => true);

            CollectionAssert.IsNotEmpty(collection.Users);
            CollectionAssert.IsEmpty(empty.Users);
        }

        [Test]
        public void GetAllUserCourses_Throws_IfUserNotFound()
        {
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _userService.GetAllUserCourses("invalid"));

            Assert.AreEqual(_userNotFound, ex?.Message);
        }

        [Test]
        public async Task GetAllUserCourses_Success_ReturnsUser()
        {
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            var actual = await _userService.GetAllUserCourses(_users[0].Id);

            Assert.AreEqual(_users[0].Id, actual.Id);
            CollectionAssert.IsNotEmpty(actual.ActiveCourses);
            CollectionAssert.IsNotEmpty(actual.PastCourses);
        }

        [Test]
        public void GetUserDetails_Throws_IfUserNotFound()
        {
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _userService.GetUserDetails("not-existing"));

            Assert.AreEqual(_userNotFound, ex?.Message);
        }

        [Test]
        public async Task GetUserDetail_Success_ReturnsUser()
        {
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            var expected = _users[0];

            var expectedFullName = expected.FirstName + " " + expected.LastName;

            var actual = await _userService.GetUserDetails(_users[0].Id);


            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expectedFullName, actual.FullName);
            Assert.AreEqual(expected.Email, actual.Email);
        }

        [Test]
        public void JoinCourse_Throws_IfCourseNotFound()
        {
            var coursesMock = _courses.BuildMock();
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);
            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _userService.JoinCourse("not-existing", "not-existing"));

            Assert.AreEqual("course not found.", ex?.Message);
        }

        [Test]
        public void JoinCourse_Throws_IfUserNotFound()
        {
            var coursesMock = _courses.BuildMock();
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);
            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _userService.JoinCourse("not-existing", _courses[0].Id));

            Assert.AreEqual(_userNotFound, ex?.Message);
        }

        [Test]
        public void JoinCourse_Throws_IfUserTryToJoinFullCourse()
        {
            var coursesMock = _courses.BuildMock();
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);
            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            var ex = Assert.CatchAsync<InvalidOperationException>(async () =>
                await _userService.JoinCourse(_users[1].Id, _courses[0].Id));

            Assert.AreEqual("Няма повече свободни места в курса.", ex?.Message);
        }

        [Test]
        public void JoinCourse_Success()
        {
            var coursesMock = _courses.BuildMock();
            var userMock = _users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);
            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(userMock);

            Assert.DoesNotThrowAsync(async () =>
                await _userService.JoinCourse(_users[1].Id, _courses[1].Id));

            Assert.AreEqual(1, _courses[1].Students.Count);
        }
    }
}
