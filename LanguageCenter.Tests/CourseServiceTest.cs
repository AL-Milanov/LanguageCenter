using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Core.Models.UserModels;
using LanguageCenter.Core.Services;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageCenter.Tests
{
    [TestFixture]
    internal class CourseServiceTest
    {
        private const double _pageResults = 6;

        private List<Language> languages;

        private List<Course> courses;

        private List<Teacher> teachers;

        private List<ApplicationUser> users;

        private readonly CourseService _courseService;

        private readonly Mock<IApplicationRepository> _applicationRepository = new();

        public CourseServiceTest()
        {
            _courseService = new(_applicationRepository.Object);
        }

        [SetUp]
        public void Setup()
        {
            languages = new List<Language>()
                {
                    new Language { Id = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", Name = "english", NormalizedName = "ENGLISH"},
                    new Language { Id = "124", Name = "Spanish", NormalizedName = "SPANISH"}
                };

            courses = new List<Course>()
            {
                new Course
            {
                Id = "valid-course",
                Title = "Английски за начинаещи",
                Level = "A1",
                Description =  "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.",
                DurationInMonths = 3,
                StartDate =  DateTime.Parse("04/06/2022"),
                EndDate = DateTime.Parse("07/09/2022"),
                LanguageId = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea",
                Language = new Language
                {
                    Id = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea",
                    Name = "english",
                    NormalizedName = "ENGLISH"
                },
                Teacher = new Teacher
                {
                    Id = "valid-teacher",
                    IsActive = true,
                    Description = "Dedicated teacher",
                    UserId = "1234567890",
                    User = new ApplicationUser
                    {
                        FirstName = "Alexander",
                        LastName = "Milanov"
                    },
                    Languages = new List<Language>()
                        {
                            new Language { Id = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", Name = "english", NormalizedName = "ENGLISH"},
                            new Language { Id = "124", Name = "Spanish", NormalizedName = "SPANISH"}
                        },
                },
                Students = new List<ApplicationUser>()
                {
                    new ApplicationUser{
                        Id = "student-1-id",
                        FirstName = "Petur",
                        LastName = "Petrov"
                    },
                    new ApplicationUser{
                        Id = "student-2-id",
                        FirstName = "Vasil",
                        LastName = "Vasilev"
                    },
                    new ApplicationUser
                    {
                        Id = "not-existing",
                        FirstName = "John",
                        LastName = "Doe"
                    }
                }
            },
                new Course
            {
                Id = "inactive-valid-course",
                Title = "Английски за начинаещи",
                Level = "A1",
                Description =  "Настоящия курс, ще ви даде базови езикови познания. Започвайки с изучаване на азбуката, трите основки групи езикови времена и други базови езикови познания.",
                DurationInMonths = 3,
                StartDate =  DateTime.Parse("04/02/2022"),
                EndDate = DateTime.Parse("07/05/2022"),
                LanguageId = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea",
                Language = new Language
                {
                    Id = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea",
                    Name = "english",
                    NormalizedName = "ENGLISH"
                },
                Teacher = new Teacher
                {
                    Id = "valid-teacher",
                    IsActive = true,
                    Description = "Dedicated teacher",
                    UserId = "1234567890",
                    User = new ApplicationUser
                    {
                        FirstName = "Alexander",
                        LastName = "Milanov"
                    },
                    Languages = new List<Language>()
                        {
                            new Language { Id = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", Name = "english", NormalizedName = "ENGLISH"},
                            new Language { Id = "124", Name = "Spanish", NormalizedName = "SPANISH"}
                        },
                }
            }
            };

            teachers = new List<Teacher>()
        {
            new Teacher
            {
                Id = "valid-teacher",
                IsActive = true,
                Description = "Dedicated teacher",
                UserId = "1234567890",
                Languages = new List<Language>()
                    {
                        new Language { Id = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", Name = "english", NormalizedName = "ENGLISH"},
                        new Language { Id = "124", Name = "Spanish", NormalizedName = "SPANISH"}
                    },
            }
        };

            users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                        Id = "student-1-id",
                        FirstName = "Petur",
                        LastName = "Petrov"
                },
                new ApplicationUser
                {
                    Id = "student-2-id",
                    FirstName = "Vasil",
                    LastName = "Vasilev"
                }
            };
        }


        [Test]
        public void AddAsync_Throws_IfLanguageNotExist()
        {
            var languageMock = languages.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languageMock);

            var addModel = new AddCourseVM()
            {
                Description = "very long description",
                DurationInMonths = 3,
                Level = "A1",
                Title = "Smart and long title",
                LanguageName = "not existing language",
                StartDate = DateTime.UtcNow,
                TeacherId = null
            };

            Assert.CatchAsync<ArgumentException>(async () => await _courseService.AddAsync(addModel), "language not found.");
        }

        [Test]
        public void AddAsync_Throws_IfCourseNotSaved()
        {
            var languageMock = languages.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languageMock);

            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(() => throw new DbUpdateException());

            var invalid = new AddCourseVM()
            {
                Description = "",
                DurationInMonths = 88,
                Level = "A",
                Title = "short",
                LanguageName = "english",
                StartDate = DateTime.UtcNow,
                TeacherId = null
            };

            Assert.CatchAsync<DbUpdateException>(async () => await _courseService.AddAsync(invalid));
        }

        [Test]
        public void AddAsync_Success_WithCorrectDate()
        {
            var languageMock = languages.BuildMock();
            var courseMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languageMock);

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(courseMock);

            var valid = new AddCourseVM()
            {
                Description = "long and valid description",
                DurationInMonths = 2,
                Level = "A1",
                Title = "Valid course name",
                LanguageName = "english",
                StartDate = DateTime.UtcNow,
                TeacherId = null,
            };

            Task.FromResult(_courseService.AddAsync(valid));
            var coursesResult = _courseService.GetAllAsync().Result;

            Assert.AreEqual(courses.Count, coursesResult.Count);
        }

        [Test]
        public void AddTeacherToCourse_Throws_IfCourseOrTeacherNotExists()
        {
            var courseMock = courses.BuildMock();
            var teacherMock = teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(courseMock);
            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.AddTeacherToCourse("invalid", "valid-teacher"), "course not found");

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.AddTeacherToCourse("valid-course", "invalid"), "teacher not found");
        }

        [Test]
        public void AddTeacherToCourse_Success()
        {
            var courseMock = courses.BuildMock();
            var teacherMock = teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(courseMock);

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teacherMock);

            var course = _courseService.AddTeacherToCourse(courses[0].Id, teachers[0].Id).Result;

            Assert.AreEqual(teachers[0], course.Teacher);
        }

        [Test]
        public void DeleteAsync_Throws_IfCantRemove()
        {

            var result = _courseService.DeleteAsync("valid-course").Result;

            Assert.AreEqual(false, result);
        }

        [Test]
        public void DeleteAsync_Success()
        {

            _applicationRepository.Setup(x => x.Delete<Course>("valid-course"))
                .Returns(Task.FromResult(true));

            var result = _courseService.DeleteAsync("valid-course").Result;

            Assert.AreEqual(true, result);
        }

        [Test]
        public void GetAll_Success()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            var expected = new List<AllCourseVM>()
            {
                new AllCourseVM()
                {
                    Id = "valid-course",
                    Title = "Английски за начинаещи",
                    Level = "A1",
                    StartDate =  "04/06/2022",
                    LanguageName = "english.png"
                }
            };

            var actual = _courseService.GetAllAsync().Result.ToList();

            Assert.AreEqual(expected[0].Id, actual[0].Id);
            Assert.AreEqual(expected[0].Title, actual[0].Title);
            Assert.AreEqual(expected[0].Level, actual[0].Level);
            Assert.AreEqual(expected[0].StartDate, actual[0].StartDate);
            Assert.AreEqual(expected[0].LanguageName, actual[0].LanguageName);

            Assert.AreEqual(courses.Count, actual.Count);
        }

        [Test]
        public void GetAllActiveAsync_Success()
        {
            var currentPage = 1;
            var pageCount = Math.Ceiling(courses.Count / _pageResults);

            var courseMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(courseMock);

            var actual = _courseService.GetAllActiveAsync(currentPage).Result;

            Assert.AreNotEqual(courses.Count, actual.Courses.Count);

            Assert.AreEqual(currentPage, actual.CurrentPage);
            Assert.AreEqual(pageCount, actual.Pages);
        }

        [Test]
        public void GetCoursesByLanguageAsync_SpanishCourses_ShouldReturnEmptyCollection()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            var expected = 0;

            var actual = _courseService.GetCoursesByLanguageAsync("spanish").Result;

            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public void GetCoursesByLanguageAsync_EnglishCourses_ShouldCollection()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            var expected = 1;

            var actual = _courseService.GetCoursesByLanguageAsync("english").Result;

            Assert.AreEqual(expected, actual.Count);
        }

        [Test]
        public void RemoveTeacherFromCourse_Throws_IfCourseNotFound()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.RemoveTeacherFromCourse("invalid"));
        }

        [Test]
        public void RemoveTeacherFromCourse_Throws_IfNotSaved()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(() => throw new DbUpdateException());

            Assert.CatchAsync<DbUpdateException>(async () =>
                await _courseService.RemoveTeacherFromCourse("valid-course"));
        }

        [Test]
        public void RemoveTeacherFromCourse_Success()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            Assert.DoesNotThrowAsync(async () =>
                await _courseService.RemoveTeacherFromCourse("valid-course"));
        }

        [Test]
        public void UpdateCourse_Throws_IfCourseNotFound()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.UpdateCourse(new EditCourseInfoVM
                {
                    Id = "invalid"
                }), "course not found");
        }

        [Test]
        public void UpdateCourse_Throws_IfNotSaved()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(() => throw new DbUpdateException());

            Assert.CatchAsync<DbUpdateException>(async () =>
                await _courseService.UpdateCourse(new EditCourseInfoVM
                {
                    Id = "valid-course",
                    Description = "New updated description",
                    DurationInMonths = 6,
                    Level = "B2",
                    StartDate = DateTime.Now,
                    Title = "New updated title"
                }));

        }

        [Test]
        public void UpdateCourse_Success()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            var newTitle = "Brand new Title";
            var newDescription = "Brand new Description";
            short newDuration = 6;
            var newLevel = "B2";
            var newStartDate = DateTime.Now;

            _courseService.UpdateCourse(new EditCourseInfoVM()
            {
                Id = "valid-course",
                Title = newTitle,
                Description = newDescription,
                DurationInMonths = newDuration,
                Level = newLevel,
                StartDate = newStartDate
            })
            .Wait();

            Assert.AreEqual(newTitle, courses[0].Title);
            Assert.AreEqual(newDescription, courses[0].Description);
            Assert.AreEqual(newDuration, courses[0].DurationInMonths);
            Assert.AreEqual(newLevel, courses[0].Level);
            Assert.AreEqual(newStartDate, courses[0].StartDate);
        }

        [Test]
        public void GetDetailsAsync_Throws_IfCourseNotFound()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.GetDetailsAsync("invalid"), "course not found");
        }

        [Test]
        public void GetDetailsAsync_Success_ReturnsCourseModel()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);


            var expected = new GetCourseVM()
            {
                Id = courses[0].Id,
                Description = courses[0].Description,
                DurationInMonths = courses[0].DurationInMonths,
                LanguageName = courses[0].Language.Name,
                Level = courses[0].Level,
                Title = courses[0].Title,
                StartDate = courses[0].StartDate.ToString("dd/MM/yyyy"),
                EndDate = courses[0].EndDate.ToString("dd/MM/yyyy"),
                TeacherName = "Alexander Milanov"
            };

            var actual = _courseService.GetDetailsAsync(courses[0].Id).Result;

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.DurationInMonths, actual.DurationInMonths);
            Assert.AreEqual(expected.LanguageName, actual.LanguageName);
            Assert.AreEqual(expected.Level, actual.Level);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.StartDate, actual.StartDate);
            Assert.AreEqual(expected.EndDate, actual.EndDate);
            Assert.AreEqual(expected.TeacherName, actual.TeacherName);
        }

        [Test]
        public void GetStudentsFromCourseAsync_Throws_IfCourseIsNotFound()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.GetStudentsFromCourseAsync("invalid"));

        }

        [Test]
        public void GetStudentsFromCourseAsync_Success_ReturnsCourse()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            var actual = _courseService.GetStudentsFromCourseAsync("valid-course").Result;

            Assert.AreEqual(courses[0].Students.Count, actual.Students.Count);
            Assert.AreEqual(courses[0].Id, actual.Id);
        }

        [Test]
        public void RemoveStudentFromCourseAsync_Throws_IfCourseNotFound()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.RemoveStudentFromCourseAsync("invalid", "student-1-id"));
        }

        [Test]
        public void RemoveStudentFromCourseAsync_Throws_IfStudentIsNotInCourse()
        {
            var coursesMock = courses.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.RemoveStudentFromCourseAsync("valid-course", "student-3-id"),
                "This student is not signed for this course.");
        }

        [Test]
        public void RemoveStudentFromCourseAsync_Throws_IfStudentNotFound()
        {
            var coursesMock = courses.BuildMock();
            var usersMock = users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);
            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(usersMock);

            Assert.CatchAsync<ArgumentException>(async () =>
                await _courseService.RemoveStudentFromCourseAsync("valid-course", "not-existing"));
        }

        [Test]
        public void RemoveStudentFromCourseAsync_Success()
        {
            var coursesMock = courses.BuildMock();
            var usersMock = users.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Course>())
                .Returns(coursesMock);

            _applicationRepository.Setup(x => x.GetAll<ApplicationUser>())
                .Returns(usersMock);

            var expected = courses[0].Students.Count - 1;

            var actual = _courseService?.RemoveStudentFromCourseAsync(courses[0].Id, users[0].Id).Result;

            Assert.AreEqual(expected, actual?.Students.Count);
            Assert.IsNull(actual?.Students.FirstOrDefault(s => s.Id == users[0].Id));
        }
    }
}
