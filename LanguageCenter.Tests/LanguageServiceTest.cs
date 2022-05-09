using LanguageCenter.Core.Models.LanguageModels;
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
    internal class LanguageServiceTest
    {
        private List<Language> _languages;

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

            Assert.That(ex?.Message, Is.EqualTo("language not found."));
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
    }
}
