﻿using Data.Models;
using Data.Tools;
using Data.Tools.Implementations;
using System;
using System.Collections.Generic;
using Tests.Integration.AuxiliaryClasses;
using Tests.Integration.Interfaces;

namespace Tests.Integration.NewsControllerTests
{
    public class NewsControllerTestData : IRepositoryControllerTestData<News, Guid>
    {
        public List<News> CreatesSuccessfullyTestCases
        {
            get
            {
                return new List<News> {
                    new News
                    {
                        NewsID = Guid.NewGuid(),
                        Title = "Test News",
                        Subtitle = "This is a test news",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                        CreationDate = DateTime.Now,
                        LastUpdated = DateTime.Now
                    },
                    new News // duplicate only to check how the yield return is structured
                    {
                        NewsID = Guid.NewGuid(),
                        Title = "(Another) Test News",
                        Subtitle = "This is another test news",
                        Content = "Hello",
                        CreationDate = DateTime.Now,
                        LastUpdated = DateTime.Now
                    }
                };
            }
        }
        public IEnumerable<Guid> DeletesByIdSuccessfullyTestData
        {
            get
            {
                return new List<Guid> {
                        new Guid("A42E0F40-D8F3-54F3-3935-075456951442")
                    };
            }
        }

        public IEnumerable<Guid> FailsDeletionIfIdDoesNotExistTestData

        {
            get
            {
                return new List<Guid> {
                        new Guid("00000000-0000-0000-0000-000000000000") // does not exist
                    };
            }
        }

        public IEnumerable<int> ListsAllRecordsSuccessfully
        {
            get
            {
                return new List<int> {
                       100
                    };
            }
        }

        public IEnumerable<GetsSpecifiedNumberOfRecordsPerPageData<News>> GetsSpecifiedNumberOfRecordsPerPage
        {
            get
            {
                return new List<GetsSpecifiedNumberOfRecordsPerPageData<News>> {
                        new GetsSpecifiedNumberOfRecordsPerPageData<News>
                        {
                            PageSize = 10,
                            PageNumber = 1,
                        },
                        new GetsSpecifiedNumberOfRecordsPerPageData<News>
                        {
                            PageSize = 10,
                            PageNumber = 2,
                        },
                        new GetsSpecifiedNumberOfRecordsPerPageData<News>
                        {
                            PageSize = 100,
                            PageNumber = 1,
                        },
                        new GetsSpecifiedNumberOfRecordsPerPageData<News>
                        {
                            PageSize = 99,
                            PageNumber = 2,
                        },
                        new GetsSpecifiedNumberOfRecordsPerPageData<News>
                        {
                            PageSize = 100,
                            PageNumber = 100,
                        },
                        new GetsSpecifiedNumberOfRecordsPerPageData<News>
                        {
                            PageSize = 70,
                            PageNumber = 2,
                        },
                };
            }
        }

        public IEnumerable<IValidationErrorsWhenCreatingData<News>> ReportsValidationErrorsWhenCreatingTestData
        {
            get
            {
                return new List<ValidationErrorsWhenCreatingData<News>> {
                    new ValidationErrorsWhenCreatingData<News> {
                        Entiy = new News {
                            NewsID = Guid.Parse("D2792985-E573-4A67-64C4-54F6ACECBBFC"), // GUID already exists, can't be used to create a new entity
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                        },
                        Message = "Validation errors occured when creating News.",
                        Errors = new string[]{ "The provided ID points to an already existing object." }
                    },
                    new ValidationErrorsWhenCreatingData<News> {
                        Entiy = new News {
                            NewsID = Guid.NewGuid(),
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now.Subtract(TimeSpan.FromSeconds(1)) // this should make it fail
                        },
                        Message = "Validation errors occured when creating News.",
                        Errors = new string[]{ "The last updated date must be more recent than the creation date." }
                    }
                };
            }
        }

        public IEnumerable<IValidationErrorsWhenCreatingData<News>> ReportsValidationErrorsWhenUpdatingTestData
        {
            get
            {
                return new List<ValidationErrorsWhenCreatingData<News>> {
                    new ValidationErrorsWhenCreatingData<News> {
                        Entiy = new News {
                            NewsID = Guid.NewGuid(), // does not exist
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                        },
                        Message = "Attempted to update or delete an entity that does not exist in the store.",
                        Errors = null // once char limits for title/subtitle/content and datetime checks are implemented, come back to this method
                    },
                    new ValidationErrorsWhenCreatingData<News> {
                        Entiy = new News {
                            NewsID = Guid.Parse("82da5e95-a4ac-436f-aba7-211a3f7343ee"), // does not exist
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                        },
                        Message = "Attempted to update or delete an entity that does not exist in the store.",
                        Errors = null // once char limits for title/subtitle/content and datetime checks are implemented, come back to this method
                    },
                    new ValidationErrorsWhenCreatingData<News> {
                        Entiy = new News {
                            NewsID = Guid.Parse("00000000-0000-0000-0000-000000000001"), // does not exist - so, funny fact. Apparently, an empty guid (the one full of zeros) will always return a default object, so that can't be used for testing, since it will make this fail. Threfore, something like this has to be used.
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                        },
                        Message = "Attempted to update or delete an entity that does not exist in the store.",
                        Errors = null // once char limits for title/subtitle/content and datetime checks are implemented, come back to this method
                    }
                };
            }
        }

        public IEnumerable<News> UpdatesSuccessfullyTestData
        {
            get
            {
                return new List<News> {
                        new News {
                            NewsID = new Guid("3E00BFDD-F9BC-FFE6-C6A9-64AFC05519B7"), // id already exists, and object is completely different
                            Title = "Test News",
                            Subtitle = "This is a test news",
                            Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vel fringilla est ullamcorper eget nulla facilisi etiam dignissim. Orci sagittis eu volutpat odio facilisis mauris sit amet massa. Tincidunt vitae semper quis lectus nulla. Accumsan tortor posuere ac ut consequat semper viverra. Dictum non consectetur a erat. Tellus molestie nunc non blandit massa enim. Mauris a diam maecenas sed. Viverra aliquet eget sit amet tellus cras. A pellentesque sit amet porttitor eget.",
                            CreationDate = DateTime.Now,
                            LastUpdated = DateTime.Now
                    }
                };
            }
        }

        public IEnumerable<IShouldSearchRecordsAppropriately<News>> ShouldSearchRecordsAppropriately
        {
            get
            {
                return new List<ShouldSearchRecordsAppropriately<News>>
                {
                    new ShouldSearchRecordsAppropriately<News>
                    {
                        PageSize = 10,
                        ColumnToSort = "date",
                        SortType = SortType.Ascending,
                        FilterOptions = new NewsFilterOptions()
                        {
                            Content = "lorem",
                            Subtitle = "et",
                            Title = "et",
                            FromDate = DateTime.Parse("2020-01-01"),
                            ToDate = DateTime.Parse("2022-01-01")
                        }
                    },
                    new ShouldSearchRecordsAppropriately<News>
                    {
                        PageSize = 5,
                        ColumnToSort = "gibberish", // should default to title sorting
                        SortType = SortType.Descending,
                        FilterOptions = new NewsFilterOptions()
                        {
                            Content = "lorem",
                            Subtitle = "ipsum",
                            ToDate = DateTime.Parse("2022-01-01")
                        }
                    },
                    new ShouldSearchRecordsAppropriately<News>
                    {
                        PageSize = 10,
                        ColumnToSort = "subtitle",
                        SortType = SortType.Ascending
                    },
                    new ShouldSearchRecordsAppropriately<News>
                    {
                        PageSize = 10,
                        ColumnToSort = "subtitle",
                        SortType = SortType.Descending
                    },
                    new ShouldSearchRecordsAppropriately<News>
                    {
                        PageSize = 20,
                        ColumnToSort = "date",
                        SortType = SortType.Descending
                    },
                    new ShouldSearchRecordsAppropriately<News>
                    {
                        PageSize = 30,
                        ColumnToSort = "title",
                        SortType = SortType.Ascending
                    },
                    new ShouldSearchRecordsAppropriately<News>
                    {
                        PageSize = 10,
                        ColumnToSort = "gibberish", // should default to title sorting
                        SortType = SortType.Ascending
                    }
                };
            }
        }
    }
}
