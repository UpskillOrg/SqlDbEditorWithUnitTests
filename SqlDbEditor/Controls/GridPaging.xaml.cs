using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SqlDbEditor.Controls
{
    /// <summary>
    /// Custom control for handling pagination in a grid.
    /// </summary>
    public partial class GridPaging
    {
        #region Dependency Properties Declarations

        /// <summary>
        /// Dependency property for TotalCount.
        /// </summary>
        public static readonly DependencyProperty TotalCountProperty;

        /// <summary>
        /// Dependency property for PageIndex.
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty;

        /// <summary>
        /// Dependency property for PageSize.
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty;

        /// <summary>
        /// Dependency property for ChangedIndexCommand.
        /// </summary>
        public static readonly DependencyProperty ChangedIndexCommandProperty;

        public static readonly DependencyProperty PreviousOrFirstPageProperty;

        public static readonly DependencyProperty NextOrLastPageProperty;

        /// <summary>
        /// Gets or sets the total count of items to be paginated.
        /// </summary>
        public int TotalCount
        {
            get => (int)GetValue(TotalCountProperty);
            set => SetValue(TotalCountProperty, value);
        }

        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        public bool PreviousOrFirstPage
        {
            get { return (bool)GetValue(PreviousOrFirstPageProperty); }
            set { SetValue(PreviousOrFirstPageProperty, value); }
        }
                  
        public bool NextOrLastPage
        {
            get { return (bool)GetValue(NextOrLastPageProperty); }
            set { SetValue(NextOrLastPageProperty, value); }
        }
        #endregion

        #region Static Constructor. Declaration of Dependency properties

        /// <summary>
        /// Static constructor that initializes the dependency properties and their metadata for the <see cref="GridPaging"/> control.
        /// </summary>
        static GridPaging()
        {
            // Metadata for the TotalCountProperty
            UIPropertyMetadata metaDataTotalCountProperty = new UIPropertyMetadata(0, PropertyTotalCountChanged);
            TotalCountProperty = DependencyProperty.Register("TotalCount", typeof(int), typeof(GridPaging), metaDataTotalCountProperty);

            // Metadata for the PageIndexProperty
            UIPropertyMetadata metaDataForPageIndexProperty = new UIPropertyMetadata(0, PropertyPageIndexChanged);
            PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(int), typeof(GridPaging), metaDataForPageIndexProperty);

            // Metadata for the PageSizeProperty
            UIPropertyMetadata metaDataForPageSizeProperty = new UIPropertyMetadata(0, PropertyPageSizeChanged);
            PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(GridPaging), metaDataForPageSizeProperty);

            PreviousOrFirstPageProperty = DependencyProperty.Register("PreviousOrFirstPage", typeof(bool), typeof(GridPaging), new PropertyMetadata(false));
            NextOrLastPageProperty = DependencyProperty.Register("NextOrLastPage", typeof(bool), typeof(GridPaging), new PropertyMetadata(false));

            // Metadata for the ChangedIndexCommandProperty
            ChangedIndexCommandProperty =
                DependencyProperty.Register(
                    "ChangedIndexCommand",
                    typeof(ICommand),
                    typeof(GridPaging),
                    new UIPropertyMetadata(null)
                );
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GridPaging"/> class.
        /// </summary>
        public GridPaging()
        {
            InitializeComponent();
            TotalCount = 0;
            PageIndex = 1;
            PageSizeComboBox.SelectedIndex = 1; // Default 100
            IsControlVisible = true;
            HasNextPage = false;
            HasPreviousPage = false;
        }

        #endregion

        #region Dependency Command Declaration

        /// <summary>
        /// Gets or sets the command to execute when the page index changes.
        /// </summary>
        public ICommand ChangedIndexCommand
        {
            get => (ICommand)GetValue(ChangedIndexCommandProperty);
            set => SetValue(ChangedIndexCommandProperty, value);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the control is visible.
        /// </summary>
        public bool IsControlVisible
        {
            get => Visibility == Visibility.Visible;
            set => Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages
        {
            get
            {
                // Calculate the number of necessary pages
                if (PageSize > 0)
                {
                    var totalPages = TotalCount / PageSize;
                    totalPages = totalPages * PageSize < TotalCount ? totalPages + 1 : totalPages;
                    return totalPages;
                }

                return 1;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage
        {
            get => FirstPageButton.IsEnabled;
            internal set => FirstPageButton.IsEnabled = FirstPageButton.IsEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage
        {
            get => LastPageButton.IsEnabled;
            internal set => LastPageButton.IsEnabled = LastPageButton.IsEnabled = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets the page index to the first page.
        /// </summary>
        public void ResetPageIndex()
        {
            PageIndex = 1;
        }

        #endregion

        #region Refactoring Configuration

        /// <summary>
        /// Configures internal control values based on the current state of the <see cref="GridPaging"/>.
        /// Sets the total number of pages, page size control, visibility of pagination buttons, and calculates the state of previous and next page buttons.
        /// </summary>
        /// <param name="gridPaging">The <see cref="GridPaging"/> instance to configure.</param>
        private static void ConfigureInternalValues(GridPaging gridPaging)
        {
            // Set the Total de paginas....
            gridPaging.TotalPageTextBlock.Text = gridPaging.TotalPages.ToString();

            // Set the pageSize control
            foreach (int comboBoxItem in gridPaging.PageSizeComboBox.Items)
            {
                int cbi = comboBoxItem;
                if (cbi == gridPaging.PageSize)
                {
                    gridPaging.PageSizeComboBox.SelectedItem = comboBoxItem;
                    break;
                }
            }

            // if the set value in Page size is not in the list, return to the original value.
            int? sel = (int)(gridPaging.PageSizeComboBox.SelectedItem ?? 0);
            if (sel != null)
            {
                gridPaging.PageSize = sel.Value;

                // Set the visibility of Pagination Buttons.
                gridPaging.PaginationGrid.Visibility = gridPaging.TotalCount > gridPaging.PageSize ?
                    Visibility.Visible :
                    Visibility.Hidden;

                // Calculate the HasNextPage and previous page
                gridPaging.HasPreviousPage = gridPaging.PageIndex > 1;
                gridPaging.HasNextPage = gridPaging.TotalPages > gridPaging.PageIndex;
            }
        }

        /// <summary>
        /// Executes the <see cref="ChangedIndexCommand"/> if it is assigned.
        /// </summary>
        private void ExecuteCommandChangeIndex()
        {
            // Test if the command index is assigned.
            if (ChangedIndexCommand != null)
            {
                ChangedIndexCommand.Execute(null);
            }
        }
        #endregion

        #region Control Events

        /// <summary>
        /// Handles the property changed event for PageSize.
        /// Recalculates control values and updates the page index.
        /// </summary>
        private static void PropertyPageSizeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GridPaging gridPaging = (GridPaging)dependencyObject;
            ConfigureInternalValues(gridPaging);
            gridPaging.PageIndex = 1;
        }

        /// <summary>
        /// Handles the property changed event for PageIndex.
        /// Updates the displayed current page and recalculates control values.
        /// </summary>
        private static void PropertyPageIndexChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GridPaging gridPaging = (GridPaging)dependencyObject;
            int actualPage = (int)e.NewValue;
            gridPaging.CurrentPageTextBlock.Text = actualPage.ToString();
            ConfigureInternalValues(gridPaging);
        }

        /// <summary>
        /// Handles the property changed event for TotalCount.
        /// Updates the total rows label, refreshes page size options, and recalculates control values.
        /// </summary>
        private static void PropertyTotalCountChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GridPaging gridPaging = (GridPaging)dependencyObject;
            var comboBox = gridPaging.PageSizeComboBox;
            gridPaging.TotalRowsTextBlock.Text = e.NewValue.ToString();
            var items = GeneratePageSizeOptions(int.Parse(gridPaging.TotalRowsTextBlock.Text));
            comboBox.ItemsSource = items;
            comboBox.SelectedItem = items.First(item => item >= 100);
            ConfigureInternalValues(gridPaging);
        }

        /// <summary>
        /// Generates a list of page size options based on the total number of records.
        /// The list includes multiples of 10 and 5 within the maxPageSize.
        /// </summary>
        private static List<int> GeneratePageSizeOptions(int totalRecords)
        {
            List<int> pageSizeOptions = new List<int>();

            // Calculate maxPageSize as the largest power of 10 <= totalRecords
            int maxPageSize = (int)Math.Pow(10, (int)Math.Log10(totalRecords));

            // Start with the smallest page size
            int pageSize = 10;

            while (pageSize <= maxPageSize)
            {
                pageSizeOptions.Add(pageSize);

                // Include multiples of 5 (e.g., 50, 500, 5000) within maxPageSize
                int nextSize = pageSize * 5;
                if (nextSize <= maxPageSize)
                {
                    pageSizeOptions.Add(nextSize);
                }

                pageSize *= 10; // Multiply the page size by 10 for the next iteration
            }

            return pageSizeOptions;
        }

        /// <summary>
        /// Handles the selection changed event for the page size combo box.
        /// Updates the page size property and executes the change index command if applicable.
        /// </summary>
        private void PageSizeChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var items = e.AddedItems;
            if (items != null && items.Count > 0)
            {
                var value = (int)items[0];
                PageSize = value;
                if (TotalCount > 0)
                {
                    ExecuteCommandChangeIndex();
                }
            }
        }

        #region Button Events for Index Control

        /// <summary>
        /// Event handler for the "Next Page" button click event.
        /// Increments the <see cref="PageIndex"/> if it's less than <see cref="TotalPages"/> and executes the <see cref="ChangedIndexCommand"/>.
        /// </summary>
        private void OnButtonNextClick(object sender, RoutedEventArgs e)
        {
            if (PageIndex < TotalPages)
            {
                PageIndex++;
                ExecuteCommandChangeIndex();
                NextOrLastPage = true;
                PreviousOrFirstPage = false;
            }
        }

        /// <summary>
        /// Event handler for the "Last Page" button click event.
        /// Sets the <see cref="PageIndex"/> to the last page and executes the <see cref="ChangedIndexCommand"/>.
        /// </summary>
        private void OnButtonLastClick(object sender, RoutedEventArgs e)
        {
            int page = TotalPages;
            PageIndex = page;
            ExecuteCommandChangeIndex();
            NextOrLastPage = true;
            PreviousOrFirstPage = false;
        }

        /// <summary>
        /// Event handler for the "Previous Page" button click event.
        /// Decrements the <see cref="PageIndex"/> if it's greater than 1 and executes the <see cref="ChangedIndexCommand"/>.
        /// </summary>
        private void OnButtonPreviousClick(object sender, RoutedEventArgs e)
        {
            if (PageIndex > 1)
            {
                PageIndex--;
                ExecuteCommandChangeIndex();
                NextOrLastPage = false;
                PreviousOrFirstPage = true;
            }
        }

        /// <summary>
        /// Event handler for the "First Page" button click event.
        /// Sets the <see cref="PageIndex"/> to the first page and executes the <see cref="ChangedIndexCommand"/>.
        /// </summary>
        private void OnButtonFirstClick(object sender, RoutedEventArgs e)
        {
            const int Page = 1;
            PageIndex = Page;
            ExecuteCommandChangeIndex();
            NextOrLastPage = false;
            PreviousOrFirstPage = true;
        }

        #endregion

        #endregion
    }
}
