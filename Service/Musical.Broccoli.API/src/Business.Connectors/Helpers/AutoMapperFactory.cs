using AutoMapper;

namespace Business.Connectors.Helpers
{
    /// <summary>
    /// Creates instances of Automapper
    /// </summary>
    public static class AutoMapperFactory
    {
        private static IMapper _mapper;

        public static IMapper Mappers
            =>
            _mapper ??
            (_mapper = new MapperConfiguration(x => { x.AddProfile(new AutoMapperConfiguration()); }).CreateMapper());
    }
}