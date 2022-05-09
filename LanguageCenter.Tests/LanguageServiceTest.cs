using LanguageCenter.Core.Models.LanguageModels;
using LanguageCenter.Core.Services;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Data.Models;
using LanguageCenter.Infrastructure.Data.Repository.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanguageCenter.Tests
{
    [TestFixture]
    internal class LanguageServiceTest
    {
        private const string languageErrorMessage = "language not found.";

        private List<Language> _languages;

        private List<Teacher> _teachers;

        private readonly ILanguageService _languageService;

        private readonly Mock<IApplicationRepository> _applicationRepository;

        public LanguageServiceTest()
        {
            _applicationRepository = new();
            _languageService = new LanguageService(_applicationRepository.Object);
        }

        [SetUp]
        public void Setup()
        {
            _languages = new List<Language>()
                {
                    new Language { Id = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", Name = "english", NormalizedName = "ENGLISH"},
                    new Language { Id = "124", Name = "Spanish", NormalizedName = "SPANISH"}
                };

            _teachers = new List<Teacher>()
            {
                new Teacher
                {
                    Id = "1234",
                    User = new()
                    {
                        Id = "123456",
                        FirstName = "Alexander",
                        LastName = "Milanov"
                    },
                    UserId = "123456",
                    Languages = new List<Language>()
                    {
                        new Language { Id = "ab32b9ec-6e77-465f-86ff-e9c4a7a296ea", Name = "english", NormalizedName = "ENGLISH"},
                    }
                },
                new Teacher
                {
                    Id = "4321",
                    User = new()
                    {
                        Id = "123336",
                        FirstName = "Petur",
                        LastName = "Petrov"
                    },
                    UserId = "123336",
                    Languages = new List<Language>()
                    {
                        new Language { Id = "124", Name = "Spanish", NormalizedName = "SPANISH"}
                    }
                },
                new Teacher
                {
                    Id = "6667",
                    User = new()
                    {
                        Id = "123466",
                        FirstName = "Georgi",
                        LastName = "Georgiev"
                    },
                    UserId = "123466",
                    Languages = new List<Language>()
                    {
                        new Language { Id = "124", Name = "Spanish", NormalizedName = "SPANISH"}
                    }
                }
            };
        }

        [Test]
        public void AddAsync_Throws_IfModelIsInvalid()
        {
            _applicationRepository.Setup(x => x.AddAsync(It.IsAny<Language>()))
                .Returns(() => throw new OperationCanceledException());

            Assert.CatchAsync<DbUpdateException>(async () =>
                await _languageService.AddAsync(new CreateLanguageVM
                {
                    Name = "german"
                }));
        }

        [Test]
        public void AddAsync_Throws_IfCantSaveChanges()
        {

            _applicationRepository.Setup(x => x.SaveChangesAsync())
                .Returns(() => throw new DbUpdateException());

            Assert.CatchAsync<DbUpdateException>(async () =>
                await _languageService.AddAsync(new CreateLanguageVM
                {
                    Name = "valid"
                }));
        }

        [Test]
        public void AddAsync_Success()
        {
            var languageMock = _languages.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languageMock);

            var before = Task.Run(() => _languageService.GetAllAsync()).Result.Count;

            Assert.AreEqual(2, before);

            var newLanguage = new CreateLanguageVM { Name = "german" };

            _languages.Add(new Language
            {
                Id = "valid-id",
                Name = newLanguage.Name,
                NormalizedName = newLanguage.Name.ToUpper(),
            });

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languageMock);

            Task.FromResult(_languageService.AddAsync(newLanguage));
            var after = Task.Run(() => _languageService.GetAllAsync()).Result.Count;

            Assert.AreEqual(3, after);
        }

        [Test]
        public void DeleteAsync_Throws_IfLanguageNotFound()
        {
            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _languageService.DeleteAsync("invalid"));

            Assert.AreEqual(languageErrorMessage, ex?.Message);
        }

        [Test]
        public async Task DeleteAsync_Success()
        {
            var languageMock = _languages.BuildMock();
            var languageToRemove = _languages[1];

            _applicationRepository.Setup(x => x.GetByIdAsync<Language>(languageToRemove.Id))
                .Returns(Task.FromResult(languageToRemove));

            _applicationRepository.Setup(x => x.Delete<Language>(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var result = await _languageService.DeleteAsync(languageToRemove.Id);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Exists_Throws_IfLanguageNotFound()
        {
            var languageMock = _languages.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languageMock);

            var ex = Assert.CatchAsync<ArgumentException>(async () =>
                await _languageService.Exists("unexisting"));

            Assert.AreEqual(languageErrorMessage, ex?.Message);
        }

        [Test]
        public async Task Exists_Success_ShouldReturnTrue()
        {
            var languagesMock = _languages.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languagesMock);

            var result = await _languageService.Exists(_languages[0].Name);

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task GetAllAsSelectedListAsync_Success_ReturnsCollection()
        {
            var languageMock = _languages.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languageMock);

            var expected = new List<SelectListItem>();

            foreach (var language in _languages)
            {
                expected.Add(new SelectListItem
                {
                    Text = language.Name,
                    Value = language.Name
                });
            }

            var actual = await _languageService.GetAllAsSelectListAsync();

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].Text, actual[i].Text);
            }
        }

        [Test]
        public async Task GetAllAsync_Success_ReturnsCollection()
        {
            var languageMock = _languages.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Language>())
                .Returns(languageMock);

            var result = await _languageService.GetAllAsync();

            Assert.AreEqual(_languages.Count, result.Count);
        }

        [Test]
        public async Task GetAllTeachersByLanguage_Success_ReturnsCollection()
        {
            var teachersMock = _teachers.BuildMock();

            _applicationRepository.Setup(x => x.GetAll<Teacher>())
                .Returns(teachersMock);

            var englishTeachers = await _languageService.GetAllTeachersByLanguage("english");

            var spanishTeachers = await _languageService.GetAllTeachersByLanguage("spaNisH");

            Assert.AreEqual(1, englishTeachers.Count);
            Assert.AreEqual(2, spanishTeachers.Count);
        }
    }
}
