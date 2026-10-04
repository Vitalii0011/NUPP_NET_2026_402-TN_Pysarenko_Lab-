using System;

namespace ClothingStore.Common
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}