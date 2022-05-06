namespace Data.Tools
{
    public static class FlashcardTools
    {
        /// <summary>
        /// Possible internal layout that can be chosen for the Flashcard contents. Values replicated from the ones existing on the front-end.
        /// </summary>
        public enum FlashcardContentLayout
        {
            SINGLE_BLOCK,
            HORIZONTAL_SPLIT,
            TRIPLE_BLOCK,
            VERTICAL_SPLIT,
            FULL_CARD
        }
    }
}
