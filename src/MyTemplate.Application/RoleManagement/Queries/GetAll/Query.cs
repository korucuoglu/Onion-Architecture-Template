namespace MyTemplate.Application.RoleManagement.Queries.GetAll;

public class Query: QueryBase<IEnumerable<Dto>>
{
    public bool WithUser { get; set; } // true olduğunda rol bilgiisi yanında kullanıcı bilgilerini de getirir. 
}