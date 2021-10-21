using System;
using Microsoft.Maui.Controls;
using FreshMvvm.Maui;

namespace FreshMvvm.Tests
{
    public static class HelperExtensions
    {
        public static Page GetPageFromNav(this Page page)
        {
            return (page as NavigationPage).CurrentPage;
        }
    }
}

