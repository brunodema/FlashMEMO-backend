using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using System.Linq;
using Xunit.Abstractions;
using Data.Tools.Sorting;
using FluentAssertions;
using Data.Models.Implementation;
using Data.Repository.Implementation;

namespace RepositoryTests.Implementation
{
    public class NewsRepositoryFixture : IDisposable
    {
        public NewsRepository _repository;
        public NewsRepositoryFixture()
        {
            var options = new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "NewsRepositoryFixture").Options;

            var context = new FlashMEMOContext(options);
            context.News.Add(new News
            {
                // all samples taken from https://goodmenproject.com/featured-content/100-examples-genuine-good-news-mostly-awesome-world-h2l/. GUID created with https://www.guidgenerator.com/online-guid-generator.aspx
                NewsID = Guid.Parse("58f59c8a-1530-4e72-95c6-4cf60ca29ddc"),
                Title = "100 Examples of Genuine Good News in a Mostly Awesome World",
                Subtitle = "We all require joy, hope and optimism. In these darker times, take heart that people care, that kindnesses, innovation and generosity are happening all over the world.",
                ThumbnailPath = "assets/features/flashmemo_dummy1.jpg",
                Content = "After the crushing and brutal massacre in San Bernardino, the terror attack in France and the shooting at a Colorado Planned Parenthood, we are in need of heartening news more than ever. Our souls require it to rehabilitate. It’s a time in our history when life can seem so out-of-control, when people appear more bent on vengeance than kindness and when they deem themselves entitled to take lives. It all coincides with our slow awakening of realizing the chasm of corruption is everywhere.",
                CreationDate = new DateTime(2019, 11, 1, 2, 13, 21),
                LastUpdated = new DateTime(2019, 11, 1, 2, 13, 21),
            });
            context.News.Add(new News
            {
                NewsID = Guid.Parse("5c2ccf9e-056f-420a-a262-e04dbe4dcab3"),
                Title = "Losing ‘Manliness’ from Aging",
                Subtitle = "Age takes its toll no matter how strong or smart or handsome you are now. Here's how you can change the way you view your masculinity.",
                ThumbnailPath = "assets/features/flashmemo_dummy2.jpg",
                Content = "A friend, who for years led African safaris, recently give me a rungu. This is a short, carved wooden staff carried by elder men of the Samburu tribe of north-central Kenya. It is a symbol of authority and honor. When a man reaches an age when he finds it difficult to go on a hunting party, he trades his weapons for a rungu. Now, rather than lend his physical strength for the well-being of the tribe, he lends his wisdom and experience.",
                CreationDate = new DateTime(2020, 7, 29, 15, 44, 1),
                LastUpdated = new DateTime(2020, 7, 29, 15, 44, 1),
            });
            context.News.Add(new News
            {
                NewsID = Guid.Parse("e17161ed-2a4d-4612-bb54-a1294a8a4e28"),
                Title = "To Some, Reparations Are Common Sense",
                Subtitle = "Three White Jeopardy contestants recently thought reparations had already been paid. It made me feel strangely optimistic.",
                ThumbnailPath = "assets/features/flashmemo_dummy3.jpg",
                Content = "A House bill to study reparations for slavery, H.R. 40, resurfaced in committee this April and is dusting up old debates about how much is owed, to whom, and who should pay.",
                CreationDate = new DateTime(2021, 1, 3, 6, 21, 55),
                LastUpdated = new DateTime(2021, 1, 3, 6, 21, 55),
            });
            context.News.Add(new News
            {
                NewsID = Guid.Parse("b12144ad-6de7-47af-9996-42178f29701b"),
                Title = "George Floyd and Friendship",
                Subtitle = "Finding hope in tragedy.",
                ThumbnailPath = "assets/features/flashmemo_dummy4.jpg",
                Content = "On April 20, 2021, I, like so many people around the world, waited with bated breath as the Derek Chauvin verdict was being announced in a quiet courtroom in Minneapolis, Minnesota. Derek Chauvin, the disgraced Minneapolis police officer who brutally assassinated George Floyd on May 25, 2020, in front of the entire world, was found guilty on all charges. My wife and I cried for joy, with the silent hope that Mr. Floyd was somewhere in Heaven celebrating the moment with his mother. Five minutes into our tear-fest I received a call from Phil Dixon, who along with his wife Cathy are two of my closest friends in the world. I heard Phil’s voice, and I broke down in tears again as I heard him quietly sobbing.Phil and I met each other in 2004.Phil is from London, England; he’s twenty some odd years older than me, and I forgot to mention that he’s white. I’m from this little suburb called Bridgeton, which is right outside of St. Louis, Missouri; I’m forty-seven, and I forgot to mention that I’m Black. We are literally from opposite ends of the planet with completely different life experiences, and yet we have forged a friendship that has weathered the highs and lows of business, life, this thing called Race in America, and everything in between.",
                CreationDate = new DateTime(2022, 10, 10, 4, 10, 05),
                LastUpdated = new DateTime(2022, 10, 10, 4, 10, 05),
            });
            context.SaveChanges();

            _repository = new NewsRepository(context);
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }

    public class NewsRepositoryTests : IClassFixture<NewsRepositoryFixture>, IBaseRepositoryTests<News, Guid>
    {
        private readonly NewsRepositoryFixture _repositoryFixture;
        private readonly ITestOutputHelper _output;

        public NewsRepositoryTests(NewsRepositoryFixture repositoryFixture, ITestOutputHelper output)
        {
            _repositoryFixture = repositoryFixture;
            _output = output;
        }
        [Fact]
        public async void CreateAsync_AssertThatItGetsProperlyCreated()
        {
            // Arrange
            var numRows = await this._repositoryFixture._repository.GetAll().CountAsync();
            var dummyNews = new News
            {
                Title = "NASA slams China after rocket debris lands near Maldives for 'failing to meet responsible standards'",
                Subtitle = "China's Long March 5B rocket made an uncontrolled reentry into Earth's atmosphere",
                Content = "NASA rebuked China for failing to meet responsible spacefaring standards after remnants of the nation's rogue Long March 5B rocket landed in the Indian Ocean near the Maldives early Sunday. Sightings of the Chinese rocket debris reentering Earth's atmosphere and scorching across the pre-dawn skies were reported in Jordan, Oman and Saudi Arabia.",
                CreationDate = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
                LastUpdated = DateTime.Now.Subtract(TimeSpan.FromDays(30))
            };

            // Act
            await this._repositoryFixture._repository.CreateAsync(dummyNews);

            // Assert
            var newNumRows = await this._repositoryFixture._repository.GetAll().CountAsync();
            Assert.True((await this._repositoryFixture._repository.GetAll().ToListAsync()).Contains(dummyNews), "Table does not contain the new item");
            Assert.True(newNumRows == numRows + 1, $"Number of rows did not increase with the new item added ({newNumRows} != {numRows + 1})");
        }
        [Fact]
        public async void UpdateAsync_AssertThatItGetsProperlyUpdated()
        {
            // Arrange
            var numRows = await this._repositoryFixture._repository.GetAll().CountAsync();
            var dummyNews = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse("b12144ad-6de7-47af-9996-42178f29701b"));

            dummyNews.Title = "New generated title, different from the previous one";
            var newTime = dummyNews.LastUpdated = DateTime.Now;

            // Act
            await this._repositoryFixture._repository.UpdateAsync(dummyNews);

            // Assert
            var newNumRows = await this._repositoryFixture._repository.GetAll().CountAsync();
            var queryResult = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse("b12144ad-6de7-47af-9996-42178f29701b"));
            Assert.NotNull(queryResult);
            Assert.True(queryResult.Title == "New generated title, different from the previous one", "Object property does not match the new updated value");
            Assert.True(queryResult.LastUpdated == newTime, "Object property does not match the new updated value");
            Assert.True(newNumRows == numRows, $"Number of rows did not stay the same with the update ({newNumRows} != {numRows})");
        }
        [Fact]
        public async void RemoveAsync_AssertThatItGetsProperlyRemoved()
        {
            // Arrange
            var numRows = await this._repositoryFixture._repository.GetAll().CountAsync();
            var dummyNews = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse("e17161ed-2a4d-4612-bb54-a1294a8a4e28"));

            // Act
            await this._repositoryFixture._repository.RemoveByIdAsync(dummyNews.NewsID);

            // Assert
            var newNumRows = await this._repositoryFixture._repository.GetAll().CountAsync();
            Assert.False((await this._repositoryFixture._repository.GetAll().ToListAsync()).Contains(dummyNews), "Table still contains the item");
            Assert.True(newNumRows == numRows - 1, $"Number of rows did not decrease with the item removed ({ newNumRows} != { numRows - 1})");
        }
        [Fact]
        public async void GetByIdAsync_AssertThatItGetsProperlyRemoved()
        {
            // Arrange
            // Act
            var dummyNews1 = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse("58f59c8a-1530-4e72-95c6-4cf60ca29ddc"));
            var dummyNews2 = await this._repositoryFixture._repository.GetByIdAsync(Guid.Parse("0d88212f-5d1e-45ab-877a-03d1b0b961ce")); // invalid GUID

            // Assert
            Assert.NotNull(dummyNews1);
            Assert.Null(dummyNews2);
        }

        [Theory]
        [InlineData(50, SortType.Ascending)]
        [InlineData(1, SortType.Ascending)]
        [InlineData(0, SortType.Ascending)]
        [InlineData(4, SortType.Ascending)]
        [InlineData(-1, SortType.Ascending)]
        [InlineData(50, SortType.Descending)]
        [InlineData(1, SortType.Descending)]
        [InlineData(0, SortType.Descending)]
        [InlineData(4, SortType.Descending)]
        [InlineData(-1, SortType.Descending)]
        public void SearchAndOrderByCreationDateAsync_ProperlyGetDataAndOrderAccordignly(int numRecords, SortType sortType)
        {
            var response = _repositoryFixture._repository.SearchAndOrderByCreationDateAsync(_ => true, sortType, numRecords);

            Assert.True(response.Count() <= (numRecords < 0 ? 0 : numRecords));
            if (sortType == SortType.Ascending)
            {
                response.Should().BeEquivalentTo(response.OrderBy(news => news.CreationDate));
            }
            else
            {
                response.Should().BeEquivalentTo(response.OrderByDescending(news => news.CreationDate));
            }
        }
    }
}

