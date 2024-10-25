namespace MyTemplate.Domain.Entities.Setting;

public class Setting : BaseEntity<int>, IEntityWithDate
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string DataType { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}