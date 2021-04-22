namespace UAA.Model
{
  /// <summary>
  /// user model
  /// </summary>
  public class UserModel
  {
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }

    public int Status { get; set; }
  }


}
