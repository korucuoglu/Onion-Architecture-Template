namespace MyTemplate.Application.ApplicationManagement.Services
{
    public interface IHashService
    {
        public string Encode(int id);

        public int Decode(string key);
    }
}