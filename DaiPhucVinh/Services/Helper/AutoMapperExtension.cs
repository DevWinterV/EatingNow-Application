using System.Collections;
using System.Collections.Generic;
using AutoMapper;

namespace DaiPhucVinh.Services.Helper
{
    public static class AutoMapperExtension
    {
        public static IMapper Mapper;
        public static List<TResult> MapTo<TResult>(this IEnumerable self)
        {
            return self == null ? (List<TResult>)null : (List<TResult>)Mapper.Map((object)self, self.GetType(), typeof(List<TResult>));
        }

        public static TResult MapTo<TResult>(this object self) where TResult : class
        {
            return self == null ? default(TResult) : (TResult)Mapper.Map(self, self.GetType(), typeof(TResult));
        }

        public static TResult MapTo<TResult>(this object self, TResult dest) where TResult : class
        {
            return self == null ? default(TResult) : (TResult)Mapper.Map(self, (object)dest, self.GetType(), typeof(TResult));
        }
    }
}
