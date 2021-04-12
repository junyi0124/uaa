namespace UAA.Model
{
  public class ReturnValue<T1, T2, TException>
  {
    public T1 Value1 { get; set; }
    public T2 Value2 { get; set; }
    public TException Exception { get; set; }
  }
}
