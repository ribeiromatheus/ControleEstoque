using AutoMapper;

namespace MyMVC
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            // Inicializa o mapeador adicionando o Profile
            Mapper.Initialize(x =>
              {
                  x.AddProfile<AutoMapperProfile>();
              });
        }
    }
}