
public interface IMapper<TInternal,TExternal>{
    TInternal MapFrom(TExternal keycloakUser);
    TExternal MapTo(TInternal internalUser);
}