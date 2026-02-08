namespace rBR.Scraper.Application.Model.Admin.View
{
    /// <summary>
    /// The class that gathers the list of dataset items of a scraper and the listing properties.
    /// </summary>
    public class ListItemsViewModel<Entity> where Entity : class
    {
        /// <summary>
        /// The <see cref="ListItemsViewModel{Entity}"/>'s constructor.
        /// </summary>
        /// <param name="dataModel">The list of objects that will be returned.</param>
        /// <param name="page">The page that is being listed.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="total">The total amount of records.</param>
        /// <param name="showing">The total amount of records that is being shown.</param>
        public ListItemsViewModel(Entity dataModel, int page, int pageSize, int total, int showing)
        {
            DatasetItems = dataModel;
            Page = page;
            PageSize = pageSize;
            Total = total;
            Showing = showing;
        }

        /// <summary>
        /// The current page that is being listed.
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// The size of the page.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// The total number of items.
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// The total amount that is being returned.
        /// </summary>
        public int Showing { get; set; }
        /// <summary>
        /// The items of the dataset that is being listed.
        /// </summary>
        public Entity DatasetItems { get; set; }
    }
}
