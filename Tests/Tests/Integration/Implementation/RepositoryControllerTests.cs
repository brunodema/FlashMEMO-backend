using Business.Tools.Validations;
using Data.Context;
using Data.Models.DTOs;
using Data.Models.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Integration.Fixtures;
using Tests.Tests.Integration.Abstract;
using Xunit;
using Xunit.Abstractions;
using static Data.Models.Implementation.StaticModels;
using static Data.Tools.FlashcardTools;
using static Tests.Tools;

namespace Tests.Tests.Integration.Implementation
{
    [Collection("Sequential")]
    public class NewsControllerTests : GenericControllerTests<News, Guid, NewsDTO>
    {
        public NewsControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

        /// <summary>
        /// Data to be used in Create, Read, and Delete tests.
        /// </summary>
        public static IEnumerable<object[]> CRDData
        {
            get
            {
                yield return new object[] { new NewsDTO { Title = "Title", Subtitle = "Subtitle", Content = "Content" } };
                yield return new object[] { new NewsDTO { Title = "Title 2", Subtitle = "Subtitle 2", Content = "Content 2", } };
                yield return new object[] { new NewsDTO { Title = "Title 3", Content = "Content 3" } };
                yield return new object[] { new NewsDTO { Subtitle = "Subtitle 4", CreationDate = DateTime.UtcNow, LastUpdated = DateTime.UtcNow } };
            }
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task CreateEntity(NewsDTO dto)
        {
            await base.CreateEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task GetEntity(NewsDTO dto)
        {
            await base.GetEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task DeleteEntity(NewsDTO dto)
        {
            await base.DeleteEntity(dto);
        }

        public static IEnumerable<object[]> UpdateEntityData
        {
            get
            {
                yield return new object[] { new NewsDTO { Title = "Title", Subtitle = "Subtitle", Content = "Content" }, new NewsDTO { Title = "Updated Title", Subtitle = "Updated Subtitle", Content = "Updated Content" } };
                yield return new object[] { new NewsDTO { Title = "Title 2", Subtitle = "Subtitle 2", Content = "Content 2", }, new NewsDTO { ThumbnailPath = "../../DoesntExistFolder/image.img" } };
            }
        }

        [Theory, MemberData(nameof(UpdateEntityData))]
        public async override Task UpdateEntity(NewsDTO dto, NewsDTO updatedDTO)
        {
            await base.UpdateEntity(dto, updatedDTO);
        }

        static List<NewsDTO> dTOs = new List<NewsDTO>()
        {
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle" },
            new NewsDTO { Title = "Spaced Title", Content = "Spaced Content", Subtitle = "Spaced Subtitle" },
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-01+00"), LastUpdated = DateTime.Parse("2000-01-01+00") },
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-01T23:59:59+00"), LastUpdated = DateTime.Parse("2000-01-01T23:59:59+00") },
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-02+00"), LastUpdated = DateTime.Parse("2000-01-02+00") },
            new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-03+00"), LastUpdated = DateTime.Parse("2000-01-03+00") },
            new NewsDTO { Title = "Title2", Content = "Content2", Subtitle = "Subtitle2" },
            new NewsDTO { Title = "Title3", Content = "Content3", Subtitle = "Subtitle3" },
            new NewsDTO { Title = "Title4", Content = "Content4", Subtitle = "Subtitle4" },
            new NewsDTO { Title = "Title5", Content = "Content5", Subtitle = "Subtitle5" },
            new NewsDTO { Title = "Title6", Content = "Content6", Subtitle = "Subtitle6" },
            new NewsDTO { Title = "Title7", Content = "Content7", Subtitle = "Subtitle7" },
            new NewsDTO { Title = "Title8", Content = "Content8", Subtitle = "Subtitle8" },
            new NewsDTO { Title = "Title9", Content = "Content9", Subtitle = "Subtitle9" },
            new NewsDTO { Title = "Title10", Content = "Content10", Subtitle = "Subtitle10" },
        };

        public static IEnumerable<object[]> ListEntityData
        {
            get
            {
                yield return new object[] { dTOs, 1 };
                yield return new object[] { dTOs, 100 };
                yield return new object[] { dTOs, 5 };
                yield return new object[] { dTOs, 7 };
                yield return new object[] { dTOs, 10 };
            }
        }

        [Theory, MemberData(nameof(ListEntityData))]
        public async override Task ListEntity(List<NewsDTO> dtoList, int pageSize)
        {
            await base.ListEntity(dtoList, pageSize);
        }

        public static IEnumerable<object[]> SearchEntityData
        {
            get
            {
                yield return new object[] { dTOs, "?title=Title2&columnToSort=title&sortType=Descending", 100, new ValidateFilteringTestData<News>() {
                    predicate = n => n.Title.Contains("Title2"),
                    sortPredicate = n => n.Title,
                    sortType = Data.Tools.Sorting.SortType.Descending
                }
            };
                yield return new object[] { dTOs, "?title=Title&orderBy=title&sortType=Ascending&columnToSort=title", 10, new ValidateFilteringTestData<News>() {
                    predicate = n => n.Title.Contains("Title"),
                    sortPredicate = n => n.Title,
                    sortType = Data.Tools.Sorting.SortType.Ascending
                }
            };
                yield return new object[] { dTOs, "?FromDate=2000-01-01&ToDate=2000-01-01&Title=Title&Subtitle=Subtitle&Content=Content", 100, new ValidateFilteringTestData<News>() {
                    predicate = n => n.Title.Contains("Title") &&
                    n.Subtitle.Contains("Subtitle") &&
                    n.Content.Contains("Content") &&
                    n.CreationDate >= DateTime.Parse("2000-01-01T00:00:00+00").ToUniversalTime() &&
                    n.CreationDate <= DateTime.Parse("2000-01-01T23:59:59+00").ToUniversalTime()
                }
            };
                yield return new object[] { dTOs, "?Title=Spaced%20Title", 100, new ValidateFilteringTestData<News>() {
                    predicate = n => n.Title.Contains("Spaced Title")
                }
            };
            }
        }

        [Theory, MemberData(nameof(SearchEntityData))]
        public async override Task SearchEntity(List<NewsDTO> dtoList, string queryParams, int pageSize, ValidateFilteringTestData<News> expectedFiltering)
        {
            await base.SearchEntity(dtoList, queryParams, pageSize, expectedFiltering);
        }

        public static IEnumerable<object[]> TestEntityValidationsData
        {
            get
            {
                yield return new object[] { new NewsDTO { Title = "Title", Content = "Content", Subtitle = "Subtitle", CreationDate = DateTime.Parse("2000-01-02+00"), LastUpdated = DateTime.Parse("2000-01-01+00") }, new List<string>() { ServiceValidationMessages.CreationDateMoreRecentThanLastUpdated }
                };
            }
        }

        [Theory, MemberData(nameof(TestEntityValidationsData))]
        public async override Task TestCreateAndUpdateValidations(NewsDTO dtoList, List<string> expectedValidations)
        {
            await base.TestCreateAndUpdateValidations(dtoList, expectedValidations);
        }
    }

    [Collection("Sequential")]
    public class DeckControllerTests : GenericControllerTests<Deck, Guid, DeckDTO>
    {
        private static readonly Language TestLanguage1 = new Language { Name = "English", ISOCode = "en" };
        private static readonly Language TestLanguage2 = new Language { Name = "French", ISOCode = "fr" };
        private static readonly Language TestLanguage3 = new Language { Name = "Italian", ISOCode = "it" };

        private static readonly ApplicationUser TestUser1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "admin", Email = "admin@flashmemo.edu" };
        private static readonly ApplicationUser TestUser2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "user", Email = "user@flashmemo.edu" };
        private static readonly ApplicationUser TestUser3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "manager", Email = "manager@flashmemo.edu" };

        public DeckControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();

                // this disgusting implementation is required because (1) no 'AddOrUpdate' method exists in the EF Core stuff anymore (despite claims of it on the internet), and because (2) the 'Update' method doesn't actually add instead of updating when providing a non-existent object (it should, though) 
                foreach (var item in new List<Language>() { TestLanguage1, TestLanguage2, TestLanguage3 })
                {
                    AddIfNecessary<Language, string>(item);
                }
                foreach (var item in new List<ApplicationUser>() { TestUser1, TestUser2, TestUser3 })
                {
                    AddIfNecessary<ApplicationUser, string>(item);
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Data to be used in Create, Read, and Delete tests.
        /// </summary>
        public static IEnumerable<object[]> CRDData
        {
            get
            {
                yield return new object[] { new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id } };
                yield return new object[] { new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage2.ISOCode, OwnerId = TestUser2.Id } };
            }
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task CreateEntity(DeckDTO dto)
        {
            await base.CreateEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task GetEntity(DeckDTO dto)
        {
            await base.GetEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task DeleteEntity(DeckDTO dto)
        {
            await base.DeleteEntity(dto);
        }

        public static IEnumerable<object[]> UpdateEntityData
        {
            get
            {
                yield return new object[] { new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id }, new DeckDTO { Name = "Deck", Description = "This is the updated description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id } };
                yield return new object[] { new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id }, new DeckDTO { Name = "Deck", Description = "This is the updated description", LanguageISOCode = TestLanguage2.ISOCode, OwnerId = TestUser2.Id } };
            }
        }

        [Theory, MemberData(nameof(UpdateEntityData))]
        public async override Task UpdateEntity(DeckDTO dto, DeckDTO updatedDTO)
        {
            await base.UpdateEntity(dto, updatedDTO);
        }

        static List<DeckDTO> dTOs = new List<DeckDTO>() {
            new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id  },
            new DeckDTO { Name = "Deck 2", Description = "This is the description 2", LanguageISOCode = TestLanguage2.ISOCode, OwnerId = TestUser2.Id  },
            new DeckDTO { Name = "Deck 3", Description = "This is the description 3", LanguageISOCode = TestLanguage3.ISOCode, OwnerId = TestUser3.Id  }
        };

        public static IEnumerable<object[]> ListEntityData
        {
            get
            {
                yield return new object[] { dTOs, 1 };
                yield return new object[] { dTOs, 100 };
                yield return new object[] { dTOs, 5 };
                yield return new object[] { dTOs, 7 };
                yield return new object[] { dTOs, 10 };
            }
        }

        [Theory, MemberData(nameof(ListEntityData))]
        public async override Task ListEntity(List<DeckDTO> dtoList, int pageSize)
        {
            await base.ListEntity(dtoList, pageSize);
        }

        public static IEnumerable<object[]> SearchEntityData
        {
            get
            {
                yield return new object[] {
                    dTOs,
                    "?name=Deck&columnToSort=name&sortType=Descending",
                    100,
                    new ValidateFilteringTestData<Deck>()
                    {
                        predicate = n => n.Name.Contains("Deck"),
                        sortPredicate = n => n.Name,
                        sortType = Data.Tools.Sorting.SortType.Descending
                    }
                };
            }
        }

        [Theory, MemberData(nameof(SearchEntityData))]
        public async override Task SearchEntity(List<DeckDTO> dtoList, string queryParams, int pageSize, ValidateFilteringTestData<Deck> expectedFiltering)
        {
            await base.SearchEntity(dtoList, queryParams, pageSize, expectedFiltering);
        }

        public static IEnumerable<object[]> TestCreateAndUpdateValidationsData
        {
            get
            {
                yield return new object[]
                {
                    new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id, CreationDate = DateTime.Parse("2000-01-02"), LastUpdated = DateTime.Parse("2000-01-01")
                    },
                    new List<string>()
                    {
                        ServiceValidationMessages.CreationDateMoreRecentThanLastUpdated
                    }
                };
                yield return new object[]
                {
                    new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = "invalid", OwnerId = TestUser1.Id,
                    },
                    new List<string>()
                    {
                        ServiceValidationMessages.InvalidLanguageCode
                    }
                };
                yield return new object[]
                {
                    new DeckDTO { Name = "Deck", Description = "This is the description", LanguageISOCode = TestLanguage1.ISOCode, OwnerId = Guid.Empty.ToString()
                    },
                    new List<string>()
                    {
                        ServiceValidationMessages.InvalidUserId
                    }
                };
            }
        }

        [Theory, MemberData(nameof(TestCreateAndUpdateValidationsData))]
        public async override Task TestCreateAndUpdateValidations(DeckDTO dto, List<string> expectedValidations)
        {
            await base.TestCreateAndUpdateValidations(dto, expectedValidations);
        }
    }

    [Collection("Sequential")]
    public class FlashcardControllerTests : GenericControllerTests<Flashcard, Guid, FlashcardDTO>
    {
        private static readonly Language TestLanguage1 = new Language { Name = "English", ISOCode = "en" };
        private static readonly Language TestLanguage2 = new Language { Name = "French", ISOCode = "fr" };
        private static readonly Language TestLanguage3 = new Language { Name = "Italian", ISOCode = "it" };

        private static readonly ApplicationUser TestUser1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "admin", Email = "admin@flashmemo.edu" };
        private static readonly ApplicationUser TestUser2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "user", Email = "user@flashmemo.edu" };
        private static readonly ApplicationUser TestUser3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "manager", Email = "manager@flashmemo.edu" };

        private static readonly Deck TestDeck1 = new Deck { DeckID = Guid.NewGuid(), LanguageISOCode = TestLanguage1.ISOCode, OwnerId = TestUser1.Id, Name = "Deck", Description = "This is a test deck" };
        private static readonly Deck TestDeck2 = new Deck { DeckID = Guid.NewGuid(), LanguageISOCode = TestLanguage2.ISOCode, OwnerId = TestUser2.Id, Name = "Deck", Description = "This is a test deck" };
        private static readonly Deck TestDeck3 = new Deck { DeckID = Guid.NewGuid(), LanguageISOCode = TestLanguage3.ISOCode, OwnerId = TestUser3.Id, Name = "Deck", Description = "This is a test deck" };

        public FlashcardControllerTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
            using (var scope = _fixture.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<FlashMEMOContext>();

                // this disgusting implementation is required because (1) no 'AddOrUpdate' method exists in the EF Core stuff anymore (despite claims of it on the internet), and because (2) the 'Update' method doesn't actually add instead of updating when providing a non-existent object (it should, though) 
                foreach (var item in new List<Language>() { TestLanguage1, TestLanguage2, TestLanguage3 })
                {
                    AddIfNecessary<Language, string>(item);
                }
                foreach (var item in new List<ApplicationUser>() { TestUser1, TestUser2, TestUser3 })
                {
                    AddIfNecessary<ApplicationUser, string>(item);
                }
                foreach (var item in new List<Deck>() { TestDeck1, TestDeck2, TestDeck3 })
                {
                    AddIfNecessary<Deck, Guid>(item);
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Data to be used in Create, Read, and Delete tests.
        /// </summary>
        public static IEnumerable<object[]> CRDData
        {
            get
            {
                yield return new object[] { new FlashcardDTO
                {
                    DeckId = TestDeck1.DeckID,
                    Level = 0, Answer = "Answer",
                    FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                    BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                    Content1 = "Front Content",
                    Content4 = "Back Content",
                    CreationDate = DateTime.Parse("2020-01-01"),
                    LastUpdated = DateTime.Parse("2020-01-01"),
                    DueDate = DateTime.Parse("2020-01-02"),
                }
                };
            }
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task CreateEntity(FlashcardDTO dto)
        {
            await base.CreateEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task GetEntity(FlashcardDTO dto)
        {
            await base.GetEntity(dto);
        }

        [Theory, MemberData(nameof(CRDData))]
        public async override Task DeleteEntity(FlashcardDTO dto)
        {
            await base.DeleteEntity(dto);
        }

        public static IEnumerable<object[]> UpdateEntityData
        {
            get
            {
                yield return new object[] { new FlashcardDTO
                {
                    DeckId = TestDeck1.DeckID,
                    Level = 0, Answer = "Answer",
                    FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                    BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                    Content1 = "Front Content",
                    Content4 = "Back Content",
                    CreationDate = DateTime.Parse("2020-01-01"),
                    LastUpdated = DateTime.Parse("2020-01-01"),
                    DueDate = DateTime.Parse("2020-01-02"),
                },
                new FlashcardDTO
                {
                    DeckId = TestDeck1.DeckID,
                    Level = 0, Answer = "Updated Answer",
                    FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                    BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                    Content1 = "Updated Front Content",
                    Content4 = "Updated Back Content",
                }
                };
            }
        }

        [Theory, MemberData(nameof(UpdateEntityData))]
        public async override Task UpdateEntity(FlashcardDTO dto, FlashcardDTO updatedDTO)
        {
            await base.UpdateEntity(dto, updatedDTO);
        }

        static List<FlashcardDTO> dTOs = new List<FlashcardDTO>() {
            new FlashcardDTO
            {
                DeckId = TestDeck1.DeckID,
                Level = 0,
                Answer = "Answer",
                FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                Content1 = "Front Content",
                Content4 = "Back Content",
                CreationDate = DateTime.Parse("2020-01-01"),
                LastUpdated = DateTime.Parse("2020-01-01"),
                DueDate = DateTime.Parse("2020-01-02"),
            },
            new FlashcardDTO
            {
                DeckId = TestDeck1.DeckID,
                Level = 1,
                Answer = "Answer",
                FrontContentLayout = FlashcardContentLayout.TRIPLE_BLOCK,
                BackContentLayout = FlashcardContentLayout.FULL_CARD,
                Content1 = "Front Content 1",
                Content2 = "https://audio.file/file.mp3",
                Content3 = "https://image.file/file.img",
                Content4 = "Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content. Back Content Back Content Back Content Back Content. Back Content Back ContentBack Content. Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content Back Content",
                Content5 = "<p>Back Content</p>",
                Content6 = "Back Content 6",
            },
            new FlashcardDTO
            {
                DeckId = TestDeck1.DeckID,
                Level = 2,
                Answer = "Answer",
                FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                Content1 = "Front Content",
                Content4 = "Back Content",
                CreationDate = DateTime.Parse("2020-01-01"),
                LastUpdated = DateTime.Parse("2020-01-01"),
            },
            new FlashcardDTO
            {
                DeckId = TestDeck1.DeckID,
                Level = 3,
                 FrontContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                BackContentLayout = FlashcardContentLayout.SINGLE_BLOCK,
                Content1 = "Front Content",
                Content4 = "Back Content",
            },
            new FlashcardDTO
            {
                DeckId = TestDeck1.DeckID,
                Level = 4,
                Answer = "Complicated Answer",
                FrontContentLayout = FlashcardContentLayout.VERTICAL_SPLIT,
                BackContentLayout = FlashcardContentLayout.HORIZONTAL_SPLIT,
                Content1 = "Front Content 1",
                Content2 = "Front Content 2",
                Content4 = "Back Content 1",
                Content5 = "Back Content 2",
            },
        };

        public static IEnumerable<object[]> ListEntityData
        {
            get
            {
                yield return new object[] { dTOs, 1 };
                yield return new object[] { dTOs, 100 };
                yield return new object[] { dTOs, 5 };
                yield return new object[] { dTOs, 7 };
                yield return new object[] { dTOs, 10 };
            }
        }

        [Theory, MemberData(nameof(ListEntityData))]
        public async override Task ListEntity(List<FlashcardDTO> dtoList, int pageSize)
        {
            await base.ListEntity(dtoList, pageSize);
        }

        public static IEnumerable<object[]> SearchEntityData
        {
            get
            {
                yield return new object[] {
                    dTOs,
                    "?answer=Complicated%20Answer&columnToSort=answer&sortType=Descending",
                    100,
                    new ValidateFilteringTestData<Flashcard>()
                    {
                        predicate = n => n.Answer.Contains("Complicated Answer"),
                        sortPredicate = n => n.Answer,
                        sortType = Data.Tools.Sorting.SortType.Descending
                    }
                };
            }
        }

        [Theory, MemberData(nameof(SearchEntityData))]
        public async override Task SearchEntity(List<FlashcardDTO> dtoList, string queryParams, int pageSize, ValidateFilteringTestData<Flashcard> expectedFiltering)
        {
            await base.SearchEntity(dtoList, queryParams, pageSize, expectedFiltering);
        }

        public static IEnumerable<object[]> TestCreateAndUpdateValidationsData
        {
            get
            {
                yield return new object[] { new FlashcardDTO { }, new List<string>() 
                {
                    "'Deck Id' must not be empty.",
                    "Main front content can not be empty.",
                    "Main back content can not be empty."
                } };
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = Guid.Empty,
                }, new List<string>() 
                {
                    "'Deck Id' must not be empty.",
                    "Main front content can not be empty.",
                    "Main back content can not be empty."
                } };
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = Guid.NewGuid(), // even though this is an invalid Id, the content ones will precede (Fluent Validations)
                }, new List<string>()
                {
                    "Main front content can not be empty.",
                    "Main back content can not be empty."
                } };
                // one test to trigger service validation around non-existing Deck
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = Guid.NewGuid(), // will generate bogus id
                    Content1 = "Content1",
                    Content4 = "Content4"
                }, new List<string>()
                {
                    ServiceValidationMessages.InvalidDeckId
                } };

                // one test to check for negative levels
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = Guid.NewGuid(), // will generate bogus id
                    Content1 = "Content1",
                    Content4 = "Content4",
                    Level = -1,
                }, new List<string>()
                {
                    "'Level' must be greater than or equal to '0'."
                } };

                // one test to check validations for complex layouts #1 (Content 2-3 and Content 5-6)
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = TestDeck1.DeckID,
                    Content1 = "Content1",
                    Content4 = "Content4",
                    FrontContentLayout = FlashcardContentLayout.HORIZONTAL_SPLIT,
                    BackContentLayout = FlashcardContentLayout.VERTICAL_SPLIT,
                }, new List<string>()
                {
                    "'Content2' must not be empty.",
                    "'Content5' must not be empty."
                } };

                // one test to check validations for complex layouts #2 (Content 2-3 and Content 5-6)
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = TestDeck1.DeckID,
                    Content1 = "Content1",
                    Content4 = "Content4",
                    FrontContentLayout = FlashcardContentLayout.TRIPLE_BLOCK,
                    BackContentLayout = FlashcardContentLayout.FULL_CARD,
                }, new List<string>()
                {
                    "'Content2' must not be empty.",
                    "'Content3' must not be empty.",
                    "'Content5' must not be empty.",
                    "'Content6' must not be empty."
                } };

                // one test to check LastUpdated GEQ CreationDate
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = TestDeck1.DeckID,
                    Content1 = "Content1",
                    Content4 = "Content4",
                    CreationDate = DateTime.Parse("2000-01-01"),
                    LastUpdated = DateTime.Parse("1999-01-01"),
                }, new List<string>()
                {
                    "'Last Updated' must be greater than or equal to '01/01/2000 00:00:00'."
                } };

                // one test to check DueDate GEQ CreationDate
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = TestDeck2.DeckID,
                    Content1 = "Content1",
                    Content4 = "Content4",
                    CreationDate = DateTime.Parse("2000-01-01"),
                    DueDate = DateTime.Parse("1999-01-01"),
                }, new List<string>()
                {
                    "'Due Date' must be greater than or equal to '01/01/2000 00:00:00'."
                } };

                // one to test both LastUpdated and DueDate
                yield return new object[]  { new FlashcardDTO
                {
                    DeckId = TestDeck3.DeckID,
                    Content1 = "Content1",
                    Content4 = "Content4",
                    CreationDate = DateTime.Parse("2000-01-01"),
                    DueDate = DateTime.Parse("1999-01-01"),
                    LastUpdated = DateTime.Parse("1999-01-01"),
                }, new List<string>()
                {
                    "'Last Updated' must be greater than or equal to '01/01/2000 00:00:00'.",
                    "'Due Date' must be greater than or equal to '01/01/2000 00:00:00'."
                } };
            }
        }

    [Theory, MemberData(nameof(TestCreateAndUpdateValidationsData))]
        public async override Task TestCreateAndUpdateValidations(FlashcardDTO dto, List<string> expectedValidations)
        {
            await base.TestCreateAndUpdateValidations(dto, expectedValidations);
        }
    }
}
