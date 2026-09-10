using CommunityToolkit.WinUI.Controls;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Threading.Tasks;

namespace EBToolbox.Views
{
    /// <summary>
    /// Shared search-highlight behavior for ConfigPage and SubSection:
    /// scrolls a located card into view and flashes its border.
    /// </summary>
    internal static class SearchHighlightHelper
    {
        public static async void RunPendingHighlightAsync(Func<string, (ItemsControl itemsControl, int index)?> locate, ScrollViewer scrollViewer)
        {
            string targetKey = App.SearchHighlightItemKey;
            if (string.IsNullOrEmpty(targetKey) || locate is null || scrollViewer is null) return;
            App.SearchHighlightItemKey = null;

            await Task.Delay(100);

            var hit = locate(targetKey);
            if (hit is not { } found) return;
            if (found.itemsControl.ContainerFromIndex(found.index) is not ContentPresenter container) return;
            SettingsCard settingsCard = FindDescendant<SettingsCard>(container);
            if (settingsCard is null) return;

            var transform = settingsCard.TransformToVisual(scrollViewer);
            var position = transform.TransformPoint(new Windows.Foundation.Point(0, 0));
            double scrollPosition = scrollViewer.VerticalOffset + position.Y - (scrollViewer.ActualHeight / 2) + (settingsCard.ActualHeight / 2);
            scrollViewer.ChangeView(null, Math.Max(0, scrollPosition), null);

            FlashCard(settingsCard);
        }

        public static T FindDescendant<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent is null) return null;

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild) return typedChild;

                var descendant = FindDescendant<T>(child);
                if (descendant is not null) return descendant;
            }
            return null;
        }

        private static void FlashCard(SettingsCard settingsCard)
        {
            var originalBrush = settingsCard.BorderBrush;
            var originalThickness = settingsCard.BorderThickness;

            var highlightBrush = new SolidColorBrush(Colors.Gold) { Opacity = 0.3 };
            settingsCard.BorderBrush = highlightBrush;
            settingsCard.BorderThickness = new Thickness(3);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1500) };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                settingsCard.BorderBrush = originalBrush;
                settingsCard.BorderThickness = originalThickness;
            };
            timer.Start();
        }
    }
}
