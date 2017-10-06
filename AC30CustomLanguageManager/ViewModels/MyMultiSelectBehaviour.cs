using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;

namespace AC30CustomLanguageManager.ViewModels
{
    public class MyMultiSelectBehaviour : Behavior<RadGridView>
    {
        private RadGridView Grid
        {
            get { return AssociatedObject as RadGridView; }
        }

        public INotifyCollectionChanged SelectedItems
        {
            get { return (INotifyCollectionChanged)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItemsProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(INotifyCollectionChanged), typeof(MyMultiSelectBehaviour), new PropertyMetadata(OnSelectedItemsPropertyChanged));

        public MyMultiSelectBehaviour() {}

        private static void OnSelectedItemsPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            var collection = args.NewValue as INotifyCollectionChanged;
            if (collection != null)
            {
                collection.CollectionChanged += ((MyMultiSelectBehaviour)target).ContextSelectedItems_CollectionChanged;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            //Grid.SelectedItems.CollectionChanged += GridSelectedItems_CollectionChanged;
            Grid.SelectedCellsChanged += Grid_SelectedCellsChanged;
        }

        void ContextSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeFromEvents();

            Transfer(SelectedItems as IList, Grid.SelectedItems);

            SubscribeToEvents();
        }


        private void Grid_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            UnsubscribeFromEvents();
            //RemoveCells(e.RemovedCells.Select(c => c.Item).ToList() as IList, SelectedItems as IList);
            //Transfer(e.AddedCells.Select(c=>c.Item).ToList() as IList, SelectedItems as IList,false);
            Transfer(Grid.SelectedCells.Select(c=>c.Item).ToList() as IList, SelectedItems as IList, true);
            SubscribeToEvents();
        }

        void GridSelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeFromEvents();

            Transfer(Grid.SelectedItems, SelectedItems as IList);

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Grid.SelectedItems.CollectionChanged += GridSelectedItems_CollectionChanged;
            Grid.SelectedCellsChanged += Grid_SelectedCellsChanged;
            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged += ContextSelectedItems_CollectionChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            Grid.SelectedItems.CollectionChanged -= GridSelectedItems_CollectionChanged;
            Grid.SelectedCellsChanged -= Grid_SelectedCellsChanged;
            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged -= ContextSelectedItems_CollectionChanged;
            }
        }
        private void RemoveCells(IList source, IList target)
        {
            try
            {
                if (source == null || target == null)
                    return;
                foreach (var o in source)
                {
                    target.Remove(o);
                }
            }
            catch { }
        }
        public static void Transfer(IList source, IList target, bool clearTargetBeforeAdd = false)
        {
            try
            {
                if (source == null || target == null)
                    return;

                if (clearTargetBeforeAdd) target.Clear();

                foreach (var o in source)
                {
                    if (!target.Contains(o))
                    {
                        target.Add(o);
                    }
                }
            }
            catch { }
        }
    }
}
