using Data;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Data.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using Data.Interfaces;
using System.Linq;

namespace RepositoryTests
{
    public class NewsRepositoryFixture : IDisposable
    {
        public NewsRepository _repository;
        public NewsRepositoryFixture()
        {
            var options = new DbContextOptionsBuilder<FlashMEMOContext>().UseInMemoryDatabase(databaseName: "FlashMEMOTest").Options;

            var context = new FlashMEMOContext(options);
            context.News.Add(new News
            {
                // all samples taken from https://goodmenproject.com/featured-content/100-examples-genuine-good-news-mostly-awesome-world-h2l/
                Title = "100 Examples of Genuine Good News in a Mostly Awesome World",
                Subtitle = "We all require joy, hope and optimism. In these darker times, take heart that people care, that kindnesses, innovation and generosity are happening all over the world.",
                ThumbnailPath = "assets/features/flashmemo_dummy1.jpg",
                Content = "After the crushing and brutal massacre in San Bernardino, the terror attack in France and the shooting at a Colorado Planned Parenthood, we are in need of heartening news more than ever. Our souls require it to rehabilitate. It’s a time in our history when life can seem so out-of-control, when people appear more bent on vengeance than kindness and when they deem themselves entitled to take lives. It all coincides with our slow awakening of realizing the chasm of corruption is everywhere.",
                CreationDate = new DateTime(2019, 11, 1, 2, 13, 21),
                LastUpdated = new DateTime(2019, 11, 1, 2, 13, 21),
            });
            context.News.Add(new News
            {
                Title = "Losing ‘Manliness’ from Aging",
                Subtitle = "Age takes its toll no matter how strong or smart or handsome you are now. Here's how you can change the way you view your masculinity.",
                ThumbnailPath = "assets/features/flashmemo_dummy2.jpg",
                Content = "A friend, who for years led African safaris, recently give me a rungu. This is a short, carved wooden staff carried by elder men of the Samburu tribe of north-central Kenya. It is a symbol of authority and honor. When a man reaches an age when he finds it difficult to go on a hunting party, he trades his weapons for a rungu. Now, rather than lend his physical strength for the well-being of the tribe, he lends his wisdom and experience.",
                CreationDate = new DateTime(2020, 7, 29, 15, 44, 1),
                LastUpdated = new DateTime(2020, 7, 29, 15, 44, 1),
            });
            context.News.Add(new News
            {
                Title = "To Some, Reparations Are Common Sense",
                Subtitle = "Three White Jeopardy contestants recently thought reparations had already been paid. It made me feel strangely optimistic.",
                ThumbnailPath = "assets/features/flashmemo_dummy3.jpg",
                Content = "A House bill to study reparations for slavery, H.R. 40, resurfaced in committee this April and is dusting up old debates about how much is owed, to whom, and who should pay.",
                CreationDate = new DateTime(2021, 1, 3, 6, 21, 55),
                LastUpdated = new DateTime(2021, 1, 3, 6, 21, 55),
            });
            context.News.Add(new News
            {
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

    public class NewsRepositoryTests : IClassFixture<NewsRepositoryFixture>
    {
        NewsRepositoryFixture _repositoryFixture;

        public NewsRepositoryTests(NewsRepositoryFixture repositoryFixture)
        {
            _repositoryFixture = repositoryFixture;
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
        public async void SearchAndOrderByCreationDateAsync_ProperlyGetDataAndOrderAccordignly(int numRecords, SortType sortType)
        {
            var response = await _repositoryFixture._repository.SearchAndOrderByCreationDateAsync(_ => true, sortType, numRecords);

            Assert.True(response.Count() <= (numRecords < 0 ? 0 : numRecords));
            if (sortType == SortType.Ascending)
            {
                Assert.True(response.OrderBy(news => news.CreationDate).SequenceEqual(response));
            }
            else
            {
                Assert.True(response.OrderByDescending(news => news.CreationDate).SequenceEqual(response));
            }
        }
    }
}

