using AutoMapper;

namespace TodoList.Application.Common.Mappings;

// 提供接口的原因是我們後面就可以在DTO裡實現各自對應的Mapping規則，方便查找。
public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}

