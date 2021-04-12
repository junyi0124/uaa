namespace UAA.Model
{
  public class ReturnValue<T, TException>
  {
    public T Value { get; set; }
    public TException Exception { get; set; }
  }
}
