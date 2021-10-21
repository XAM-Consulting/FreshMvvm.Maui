using System;

namespace FreshMvvm.Maui
{
    public interface IFreshPageModelMapper
    {
        string GetPageTypeName(Type pageModelType);
    }
}

