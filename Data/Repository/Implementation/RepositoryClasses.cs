using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Data.Context;
using Data.Tools.Sorting;
using Data.Repository.Abstract;
using Data.Models.Implementation;
using static Data.Models.Implementation.StaticModels;

namespace Data.Repository.Implementation
{
    public class NewsRepository : GenericRepository<News, Guid, FlashMEMOContext>
    {
        public NewsRepository(FlashMEMOContext context) : base(context) { }

        public IEnumerable<News> SearchAndOrderByCreationDateAsync(Expression<Func<News, bool>> predicate, SortType sortType, int numRecords)
        {
            return base.SearchAndOrder(predicate, new NewsSortOptions(sortType, NewsSortOptions.ColumnOptions.DATE), numRecords);
        }
    }

    public class DeckRepository : GenericRepository<Deck, Guid, FlashMEMOContext>
    {
        public DeckRepository(FlashMEMOContext context) : base(context) { }
    }

    public class FlashcardRepository : GenericRepository<Flashcard, Guid, FlashMEMOContext>
    {
        public FlashcardRepository(FlashMEMOContext context) : base(context) { }
    }

    /// <summary>
    /// Even though this entity is not supposed to be managed indirectly, it certainly helps to use the existing interfaces to set up a repository for it.
    /// </summary>
    public class LanguageRepository : GenericRepository<Language, Guid, FlashMEMOContext>
    {
        public LanguageRepository(FlashMEMOContext context) : base(context) { }
    }
}
