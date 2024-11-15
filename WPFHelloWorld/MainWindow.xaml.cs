using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FinancialReview
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<FinancialReviewItem> FinancialReviewItems { get; set; }
        private ObservableCollection<DataGridColumn> CustomColumns { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FinancialReviewItems = new ObservableCollection<FinancialReviewItem>();
            CustomColumns = new ObservableCollection<DataGridColumn>();

            // Sample data initialization
            InitializeSampleData();

            // Set DataContext
            this.DataContext = this;

            // Load mock data
            LoadMockData();

            // Bind the ComboBox to CustomColumns collection
            CustomColumnsComboBox.ItemsSource = CustomColumns;
        }

        public void GenerateColumns(IEnumerable<string> propertyNames)
        {
            // Clear existing columns except for standard ones
            var standardColumnHeaders = new HashSet<string>
            {
                "Select",
                "Status",
                "Request Name",
                "Created on",
                "Client",
                "Days Remaining",
                "Days Outstanding",
                "Document Count"
            };

            // Remove existing dynamic columns
            var columnsToRemove = FinancialDataGrid.Columns
                .Where(c => !standardColumnHeaders.Contains(c.Header.ToString()))
                .ToList();

            foreach (var column in columnsToRemove)
            {
                FinancialDataGrid.Columns.Remove(column);
            }

            // Generate columns for dynamic properties
            foreach (var propertyName in propertyNames)
            {
                // Skip if it's a standard property
                if (standardColumnHeaders.Contains(propertyName))
                    continue;

                // Create a new column
                DataGridTextColumn customColumn = new DataGridTextColumn
                {
                    Header = propertyName,
                    Binding = new Binding($"[{propertyName}]"),
                    Width = DataGridLength.Auto
                };

                // Add the column to the DataGrid
                FinancialDataGrid.Columns.Add(customColumn);
            }
        }


        private void InitializeSampleData()
        {
            // Add sample data items
            FinancialReviewItems.Add(new FinancialReviewItem
            {
                IsSelected = false,
                Status = "Open",
                RequestName = "Bank Statements",
                CreatedOn = DateTime.Now.AddDays(-10),
                Client = "audit.client@yzcompany.com",
                DaysRemaining = 20,
                DaysOutstanding = 5,
                DocumentCount = 3
            });

            FinancialReviewItems.Add(new FinancialReviewItem
            {
                IsSelected = false,
                Status = "In Review",
                RequestName = "Invoices",
                CreatedOn = DateTime.Now.AddDays(-40),
                Client = "audit.client@yzcompany.com",
                DaysRemaining = 5,
                DaysOutstanding = 0,
                DocumentCount = 3
            });

            FinancialReviewItems.Add(new FinancialReviewItem
            {
                IsSelected = false,
                Status = "Complete",
                RequestName = "Purchase Orders",
                CreatedOn = DateTime.Now.AddDays(-120),
                Client = "audit.client@yzcompany.com",
                DaysRemaining = 5,
                DaysOutstanding = 0,
                DocumentCount = 3
            });

            FinancialReviewItems.Add(new FinancialReviewItem
            {
                IsSelected = false,
                Status = "Open",
                RequestName = "A/R Reconciliation",
                CreatedOn = DateTime.Now.AddDays(-87),
                Client = "audit.client@yzcompany.com",
                DaysRemaining = 5,
                DaysOutstanding = 0,
                DocumentCount = 3
            });

            FinancialReviewItems.Add(new FinancialReviewItem
            {
                IsSelected = false,
                Status = "Open",
                RequestName = "List of prepaid accounts",
                CreatedOn = DateTime.Now.AddDays(-220),
                Client = "audit.client@yzcompany.com",
                DaysRemaining = 5,
                DaysOutstanding = 0,
                DocumentCount = 3
            });

            FinancialReviewItems.Add(new FinancialReviewItem
            {
                IsSelected = false,
                Status = "Open",
                RequestName = "List of prepaid accounts",
                CreatedOn = DateTime.Now.AddDays(-110),
                Client = "audit.client@yzcompany.com",
                DaysRemaining = 5,
                DaysOutstanding = 0,
                DocumentCount = 3
            });
            // Add more items as needed
        }

        // Method to add a custom column
        public void AddCustomColumn(string columnName)
        {
            // Add the property to all data items
            foreach (var item in FinancialReviewItems)
            {
                item[columnName] = null; // Initialize with null or default value
            }

            // Create a new column
            DataGridTextColumn customColumn = new DataGridTextColumn
            {
                Header = columnName,
                Binding = new Binding($"[{columnName}]"),
                Width = DataGridLength.Auto
            };

            // Add the column to the DataGrid
            FinancialDataGrid.Columns.Add(customColumn);

            // Add the column to the custom columns collection
            CustomColumns.Add(customColumn);
        }

        private void AddColumnButton_Click(object sender, RoutedEventArgs e)
        {
            string newColumnName = CustomColumnNameTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(newColumnName))
            {
                // Check for duplicate column names
                if (FinancialDataGrid.Columns.Any(c => c.Header.ToString() == newColumnName))
                {
                    MessageBox.Show("A column with this name already exists.", "Duplicate Column", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Add the custom column
                AddCustomColumn(newColumnName);

                // Clear the input
                CustomColumnNameTextBox.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Please enter a valid column name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomColumnsComboBox.SelectedItem is DataGridColumn selectedColumn)
            {
                string columnName = selectedColumn.Header.ToString();

                // Remove the column from the DataGrid
                FinancialDataGrid.Columns.Remove(selectedColumn);

                // Remove the column from the custom columns collection
                CustomColumns.Remove(selectedColumn);

                // Remove the property from data items
                foreach (var item in FinancialReviewItems)
                {
                    if (item.ContainsProperty(columnName))
                    {
                        item.RemoveProperty(columnName);
                    }
                }

                // Clear selection
                CustomColumnsComboBox.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Please select a custom column to delete.", "No Column Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CustomColumnNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FinancialDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CustomColumnNameTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void LoadMockData()
        {
            // Create a new ObservableCollection for the financial review items
            var financialReviewItems = new ObservableCollection<FinancialReviewItem>();

            // A HashSet to collect all unique property names
            var allPropertyNames = new HashSet<string>();

            // Item 1
            var item1 = new FinancialReviewItem
            {
                IsSelected = false,
                Status = "Open",
                RequestName = "Review Q1",
                CreatedOn = DateTime.Now.AddDays(-10),
                Client = "Client A",
                DaysRemaining = 20,
                DaysOutstanding = 5,
                DocumentCount = 3
            };
            // Add dynamic properties
            item1["CustomProp1"] = "Value1";
            item1["CustomProp2"] = 100;
            allPropertyNames.Add("CustomProp1");
            allPropertyNames.Add("CustomProp2");

            financialReviewItems.Add(item1);

            // Item 2
            var item2 = new FinancialReviewItem
            {
                IsSelected = false,
                Status = "Closed",
                RequestName = "Review Q2",
                CreatedOn = DateTime.Now.AddDays(-40),
                Client = "Client B",
                DaysRemaining = 0,
                DaysOutstanding = 0,
                DocumentCount = 0
            };
            // Add dynamic properties
            item2["CustomProp2"] = 200;
            item2["CustomProp3"] = "Value3";
            allPropertyNames.Add("CustomProp2");
            allPropertyNames.Add("CustomProp3");

            financialReviewItems.Add(item2);

            // Item 3
            var item3 = new FinancialReviewItem
            {
                IsSelected = false,
                Status = "Pending",
                RequestName = "Review Q3",
                CreatedOn = DateTime.Now.AddDays(-5),
                Client = "Client C",
                DaysRemaining = 15,
                DaysOutstanding = 2,
                DocumentCount = 1
            };
            // Add dynamic properties
            item3["CustomProp4"] = DateTime.Now;
            allPropertyNames.Add("CustomProp4");

            financialReviewItems.Add(item3);

            // Update the FinancialReviewItems collection
            FinancialReviewItems = financialReviewItems;

            // Generate columns based on the collected property names
            GenerateColumns(allPropertyNames);

            // Refresh the DataGrid's ItemsSource
            FinancialDataGrid.ItemsSource = FinancialReviewItems;
        }
    }
}
