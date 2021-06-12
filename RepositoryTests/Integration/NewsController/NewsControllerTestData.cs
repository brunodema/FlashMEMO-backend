using Data.Models;
using System;
using System.Collections.Generic;
using Tests.Integration.Interfaces;

namespace Tests.Integration.NewsTests
{
    public class NewsControllerTestData : IRepositoryControllerTestData<News>
    {
        public static IEnumerable<object[]> CreatesSuccessfullyTestCases
        {
            get
            {
                yield return new object[] {
                    new News {
                        NewsID = Guid.NewGuid(),
                        Title = "Test News",
                        Subtitle = "This is a test news",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                        CreationDate = DateTime.Now,
                        LastUpdated = DateTime.Now
                    }
                };
            }
        }
        public static IEnumerable<object[]> DeletesByIdSuccessfullyTestData
        {
            get
            {
                yield return new object[] {
                        new Guid("5CDA2C98-98D7-0341-0D7F-5F634136DBE3")
                    };
            }
        }

        public static IEnumerable<object[]> FailsDeletionIfIdDoesNotExistTestData

        {
            get
            {
                yield return new object[] {
                        new Guid("00000000-0000-0000-0000-000000000000")
                    };
            }
        }

        public static IEnumerable<object[]> ReportsValidationErrorsWhenCreatingTestData
        {
            get
            {
                yield return new object[] {
                        new News {
                            NewsID = Guid.NewGuid(),
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                        },
                        new string[]{} // once char limits for title/subtitle/content and datetime checks are implemented, come back to this method
                    };
            }
        }

        public static IEnumerable<object[]> ReportsValidationErrorsWhenUpdatingTestData
        {
            get
            {
                yield return new object[] {
                        new News {
                            NewsID = Guid.NewGuid(),
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                        },
                        new string[]{} // once char limits for title/subtitle/content and datetime checks are implemented, come back to this method
                    };
            }
        }

        public static IEnumerable<object[]> UpdatesSuccessfullyTestData
        {
            get
            {
                yield return new object[] {
                        new News {
                            NewsID = new Guid("3C976BBA-BFF7-0EF5-5A6B-B0AE96F7D3F2"), // id already exists, and object is completely different
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                    }
                };
            }
        }
    }
}
