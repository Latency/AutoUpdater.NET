// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Delegates.cs
// Author:   Latency McLaughlin
// Date:     02/20/2026
// ****************************************************************************

using System.Windows.Controls;

namespace AutoUpdaterDotNET.Extensions;

internal static class Delegates
{
    extension(TreeViewItem item)
    {
        public TreeViewItem Header(string name)
        {
            item.Header = $"On{name.Replace("NodeList", string.Empty)} (Delegates)";
            return item;
        }
    }


    extension(ItemCollection collection)
    {
        public void RemoveDelegate(Delegate value)
        {
            var node = collection.Cast<TreeViewItem>().FirstOrDefault(item => item.Header!.ToString()!.Split('.')[1].TrimStart().Equals(value.Method.Name));
            if (node != null)
                collection.Remove(node);

            if (collection.IsEmpty)
                collection.Add(new TreeViewItem
                {
                    Header = "(None)"
                });
        }

        public void AddDelegate(Delegate? action)
        {
            collection.Add(action is null ? new()
            {
                Header = "(None)"
            } : new TreeViewItem
            {
                Header = $"{collection.Count + 1}.  {action.Method.Name}"
            });
        }
    }
}