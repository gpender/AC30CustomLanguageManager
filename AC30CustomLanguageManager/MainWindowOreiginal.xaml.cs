using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace AC30CustomLanguageManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(MainWindow_PreviewKeyDown);
            this.grid1.PreviewMouseRightButtonUp += Grid1_PreviewMouseRightButtonUp;
        }

        private void Grid1_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var s = e.OriginalSource as FrameworkElement;
            if (s is TextBlock)
            {
                var parentCell = s.ParentOfType<GridViewCell>();
                if (parentCell != null)
                {
                    //parentCell.IsSelected = true;
                    //this.grid1.SelectedCells.Add
                }
            }
        }

        void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!grid1.Items.IsEditingItem)
                return;

            this.HandleKeyDown(e);
            this.HandleKeyUp(e);
            this.HandleKeyLeft(e);
            this.HandleKeyRight(e);
            this.HandlePrev(e);
            this.HandleNext(e);
        }

        private void HandleKeyRight(KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                var editBox = grid1.CurrentCell.ChildrenOfType<TextBox>().FirstOrDefault();

                if (editBox !=null && editBox.CaretIndex == editBox.Text.Length)
                {
                    this.Dispatcher.BeginInvoke((Action)(() => RadGridViewCommands.MoveRight.Execute(null)));
                    this.Dispatcher.BeginInvoke((Action)(() => RadGridViewCommands.SelectCurrentUnit.Execute(null)));
                    this.Dispatcher.BeginInvoke((Action)(() => RadGridViewCommands.BeginEdit.Execute(null)));

                    e.Handled = true;
                }
            }
        }
        private void HandleKeyLeft(KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                var editBox = grid1.CurrentCell.ChildrenOfType<TextBox>().FirstOrDefault();

                if (editBox !=null && editBox.CaretIndex == 0)
                {
                    RadGridViewCommands.MoveLeft.Execute(null);
                    RadGridViewCommands.SelectCurrentUnit.Execute(null);
                    RadGridViewCommands.BeginEdit.Execute(null);

                    e.Handled = true;
                }
            }
        }
        private void HandleKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                RadGridViewCommands.MoveUp.Execute(null);
                RadGridViewCommands.SelectCurrentUnit.Execute(null);
                RadGridViewCommands.BeginEdit.Execute(null);
                FocusEditTextBox();

                e.Handled = true;
            }
        }
        private void FocusEditTextBox()
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                var editBox = grid1.CurrentCell.GetEditingElement() as TextBox;
                if (editBox != null && editBox.IsLoaded)
                {
                    editBox.Focus();
                }
                else
                {
                    grid1.CurrentCell.GotFocus += new RoutedEventHandler(CurrentCell_GotFocus);
                }
            }));
        }
        void CurrentCell_GotFocus(object sender, RoutedEventArgs e)
        {
            if (grid1.CurrentCell.IsInEditMode)
            {
                var editBox = grid1.CurrentCell.GetEditingElement() as TextBox;
                if (editBox != null && editBox != null)
                {
                    editBox.SelectAll();
                    editBox.Focus();
                }
            }
        }
        private void HandleKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                RadGridViewCommands.MoveDown.Execute(null);
                RadGridViewCommands.SelectCurrentUnit.Execute(null);
                RadGridViewCommands.BeginEdit.Execute(null);
                FocusEditTextBox();

                e.Handled = true;
            }
        }
        private void HandleNext(KeyEventArgs e)
        {
            if (e.Key == Key.PageDown)
            {
                RadGridViewCommands.MovePageDown.Execute(null);
                e.Handled = true;
            }
        }
        private void HandlePrev(KeyEventArgs e)
        {
            if (e.Key == Key.PageUp)
            {
                RadGridViewCommands.MovePageUp.Execute(null);
                e.Handled = true;
            }
        }

        private void RadContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    RadContextMenu menu = (RadContextMenu)sender;
            //    GridViewCell cell = menu.GetClickedElement<GridViewCell>();
            //    if (cell != null)
            //    {
            //        cell.IsSelected = true;
            //        cell.IsCurrent = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }

    }
}
