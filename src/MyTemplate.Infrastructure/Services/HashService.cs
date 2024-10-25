using HashidsNet;
using MyTemplate.Application.ApplicationManagement.Services;

namespace MyTemplate.Infrastructure.Services;

public class HashService : IHashService
{
    private readonly Hashids _hashids;

    public HashService()
    {
        _hashids = new Hashids("my_saltkey", 8);
    }

    public string Encode(int id)
    { 
        return _hashids.Encode(id);
    }

    public int Decode(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        
        return _hashids.DecodeSingle(key);
    }
}