using LanguageCenter.Core.Services;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
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
    internal class UserServiceTest
    {

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

            Assert.AreEqual("user not found.", ex?.Message);
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
    }
}
